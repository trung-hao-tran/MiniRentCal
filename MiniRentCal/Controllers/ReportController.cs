using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniRentCal.Data;
using MiniRentCal.Models;
using System.Text.Json;

namespace MiniRentCal.Controllers
{
    public class ReportController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            // Fetch calculation sessions sorted by date
            var sessions = await _context.CalculationSessions
                .OrderByDescending(s => s.CalculationDate)
                .ToListAsync();

            return View(sessions); // Pass directly to the view
        }

        // GET: SummaryPartial
        public async Task<IActionResult> SummaryPartial(int sessionId)
        {
            // Fetch the specified calculation session
            var session = await _context.CalculationSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                return NotFound("Calculation session not found.");
            }

            // Deserialize the stored unit data
            var rawBlocksData = JsonSerializer.Deserialize<List<RentalBlockViewModel>>(session.SerializedUnits);

            ViewBag.CalculationDate = session.CalculationDate.ToString("dd/MM/yyyy");
            ViewBag.TotalCost = session.TotalCost;
            ViewBag.TotalUnits = session.TotalUnits;
            ViewBag.SessionId = session.Id;

            return PartialView("_SummaryPartial", rawBlocksData);
        }

        

    }
}
