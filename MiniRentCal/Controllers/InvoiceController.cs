using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniRentCal.Data;
using MiniRentCal.Models;
using QuestPDF.Fluent;
using System.Text.Json;

namespace MiniRentCal.Controllers
{
    public class InvoiceController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IActionResult> GenerateInvoices(int? sessionId = null)
        {
            // Fetch the session based on the ID or get the latest one
            var session = sessionId.HasValue
                ? await _context.CalculationSessions.FirstOrDefaultAsync(s => s.Id == sessionId)
                : await _context.CalculationSessions.OrderByDescending(s => s.CalculationDate).FirstOrDefaultAsync();

            if (session == null)
            {
                return NotFound("Calculation session not found.");
            }

            // Deserialize the stored unit data
            var rawBlocksData = JsonSerializer.Deserialize<List<RentalBlockViewModel>>(session.SerializedUnits);

            var invoices = rawBlocksData
                .SelectMany(block => block.Units.Select(unit => new InvoiceData
                {
                    BlockNumber = block.BlockNumber,
                    RoomNumber = unit.RoomNumber,
                    Date = session.CalculationDate,
                    NewElectricAmount = unit.NewElectricAmount,
                    OldElectricAmount = unit.CurrentElectricAmount,
                    ElectricCost = unit.ElectricCost,
                    NewWaterAmount = unit.NewWaterAmount,
                    OldWaterAmount = unit.CurrentWaterAmount,
                    WaterCost = unit.WaterCost,
                    GarbageCost = unit.GarbageCost,
                    RoomCost = unit.UnitCost
                }))
                .ToList();

            var document = new InvoiceDocument(invoices);
            var pdfStream = document.GeneratePdf();

            var formattedDate = session.CalculationDate.ToString("dd-MM-yyyy");
            var fileName = $"hoá đơn - {formattedDate}.pdf";

            return File(pdfStream, "application/pdf", fileName);
        }

    }
}
