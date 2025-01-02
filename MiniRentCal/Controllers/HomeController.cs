using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniRentCal.Data;
using MiniRentCal.Models;

namespace MiniRentCal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Retrieve all units
            var units = _context.Units.OrderBy(b => b.BlockNumber).ToList();

            // Check if utility costs exist
            var utilCost = _context.Utilities.FirstOrDefault();
            if (utilCost == null)
            {
                // Create a default utility cost record
                utilCost = new UtilityModel
                {
                    ElectricCost = 0,
                    WaterCost = 0,
                    GarbageCost = 0
                };

                // Save the default utility cost record to the database
                _context.Utilities.Add(utilCost);
                _context.SaveChanges();
            }

            // Pass the utility cost to the view
            ViewData["utility"] = utilCost;

            return View(units);
        }


        [HttpPost]
        public IActionResult Create(UnitModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Units.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult Edit(UnitModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Units.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateUtility(UtilityModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Utilities.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id) 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            UnitModel? current = _context.Units.Find(id);
            if (current != null)
            {
                _context.Units.Remove(current);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
