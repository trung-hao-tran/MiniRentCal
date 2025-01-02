using Microsoft.AspNetCore.Mvc;
using MiniRentCal.Data;
using MiniRentCal.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using QuestPDF.Fluent;

namespace MiniRentCal.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalculatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> CalculateRental()
        {
            var selectedRoomIds = JsonSerializer.Deserialize<List<int>>(TempData["SelectedRoomIds"]?.ToString() ?? "[]");
            var units = await _context.Units
                        .Where(u => selectedRoomIds.Contains(u.Id))
                        .OrderBy(b => b.BlockNumber)
                        .ToListAsync();
            var utility = await _context.Utilities.FirstOrDefaultAsync();

            if (utility == null)
            {
                utility = new UtilityModel
                {
                    ElectricCost = 0,
                    WaterCost = 0,
                    GarbageCost = 0
                };
                if (!await _context.Utilities.AnyAsync()) // Ensure only one default utility model exists
                {
                    _context.Utilities.Add(utility);
                    await _context.SaveChangesAsync();
                }
            }

            var blocks = units.GroupBy(u => u.BlockNumber)
                .Select(group => new RentalBlockViewModel
                {
                    BlockNumber = group.Key,
                    Units = group.Select(u => new RentalCalculationViewModel
                    {
                        UnitId = u.Id,
                        RoomNumber = u.RoomNumber,
                        UnitCost = u.Cost,
                        CurrentElectricAmount = u.CurrentElectricAmount,
                        CurrentWaterAmount = u.CurrentWaterAmount,
                        ElectricCost = utility.ElectricCost,
                        WaterCost = utility.WaterCost,
                        GarbageCost = utility.GarbageCost,
                        CalculationDate = DateTime.Now
                    }).ToList()
                }).ToList();

            return View(blocks);
        }

        [HttpPost]
        public async Task<ActionResult> CalculateRental(List<RentalBlockViewModel> blocks)
        {
            if (!ModelState.IsValid)
            {
                return View(blocks);
            }

            // Serialize all unit details across all blocks into JSON
            var allUnits = blocks.Select(block => new
            {
                BlockNumber = block.BlockNumber,
                Units = block.Units.Select(unit => new
                {
                    unit.RoomNumber,
                    unit.UnitCost,
                    unit.CurrentElectricAmount,
                    unit.NewElectricAmount,
                    unit.CurrentWaterAmount,
                    unit.NewWaterAmount,
                    unit.ElectricCost,
                    unit.WaterCost,
                    unit.GarbageCost,
                    unit.TotalCost
                }).ToList()
            }).ToList();

            var serializedUnits = System.Text.Json.JsonSerializer.Serialize(allUnits);
            var totalCost = blocks.Sum(block => block.Units.Sum(unit => unit.TotalCost));
            var totalUnits = blocks.Sum(block => block.Units.Count);

            // Create a new calculation session
            var calculationSession = new CalculationSession
            {
                CalculationDate = DateTime.Now,
                TotalCost = totalCost,
                TotalUnits = totalUnits,
                SerializedUnits = serializedUnits
            };

            _context.CalculationSessions.Add(calculationSession);
            await _context.SaveChangesAsync();

            TempData["Blocks"] = JsonSerializer.Serialize(blocks);
            return RedirectToAction("Summary", blocks);
        }

        public async Task<IActionResult> Summary()
        {
            // Fetch the most recent calculation session
            var session = await _context.CalculationSessions
                .OrderByDescending(s => s.CalculationDate)
                .FirstOrDefaultAsync();

            if (session == null)
            {
                return NotFound("No calculation session found.");
            }

            // Deserialize the stored unit data
            var rawBlocksData = System.Text.Json.JsonSerializer.Deserialize<List<RentalBlockViewModel>>(session.SerializedUnits);

            // Pass metadata and raw block data to the view
            ViewBag.CalculationDate = session.CalculationDate.ToString("dd/MM/yyyy");
            ViewBag.TotalCost = session.TotalCost;
            ViewBag.TotalUnits = session.TotalUnits;
            ViewBag.SessionId = session.Id;

            return View(rawBlocksData);
        }

        // GET: Utility/ConfirmCosts
        public async Task<ActionResult> ConfirmUtilCosts()
        {
            var utility = await _context.Utilities.FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new UtilityModel
                {
                    ElectricCost = 0,
                    WaterCost = 0,
                    GarbageCost = 0
                };
                if (!await _context.Utilities.AnyAsync()) // Ensure only one default utility model exists
                {
                    _context.Utilities.Add(utility);
                    await _context.SaveChangesAsync();
                }
            }

            return View(utility);
        }

        // POST: Utility/SaveUtilityCosts
        [HttpPost]
        public async Task<ActionResult> SaveUtilityCosts(UtilityModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmCosts", model);
            }

            var utility = await _context.Utilities.FirstOrDefaultAsync();
            if (utility != null)
            {
                utility.ElectricCost = model.ElectricCost;
                utility.WaterCost = model.WaterCost;
                utility.GarbageCost = model.GarbageCost;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ConfirmRoom");
        }

        // GET: Calculator/ConfirmRoom
        public async Task<IActionResult> ConfirmRoom()
        {
            // Fetch units grouped by blocks
            var units = await _context.Units
                .OrderBy(u => u.BlockNumber)
                .ThenBy(u => u.RoomNumber)
                .GroupBy(u => u.BlockNumber)
                .Select(group => new ConfirmRoomViewModel
                {
                    BlockNumber = group.Key,
                    Rooms = group.Select(u => new ConfirmRoomUnitViewModel
                    {
                        UnitId = u.Id,
                        RoomLabel = $"{u.BlockNumber}{u.RoomNumber}",
                        IsSelected = true // Default all rooms to selected
                    }).ToList()
                }).ToListAsync();

            return View(units);
        }

        // POST: Calculator/ConfirmRoom
        [HttpPost]
        public IActionResult ConfirmRoom(List<int> SelectedRoomIds)
        {
            if (SelectedRoomIds == null || !SelectedRoomIds.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một phòng.");
                return RedirectToAction("ConfirmRoom");
            }

            // Store selected room IDs for the next stage
            TempData["SelectedRoomIds"] = JsonSerializer.Serialize(SelectedRoomIds);

            return RedirectToAction("CalculateRental");
        }

    }
}