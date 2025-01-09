namespace MiniRentCal.Models
{
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;

    public class OwnerInvoiceDocument : IDocument
    {
        private readonly List<InvoiceData> _invoices;

        public OwnerInvoiceDocument(List<InvoiceData> invoices)
        {
            _invoices = invoices;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        [Obsolete]
        public void Compose(IDocumentContainer container)
        {
            var blocks = _invoices.GroupBy(i => i.BlockNumber);

            foreach (var block in blocks)
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4); // Standard A4 page size
                    page.Margin(10);         // Outer margin

                    // Add Block and Month/Year Header
                    page.Header().PaddingBottom(10).Column(column =>
                    {
                        var monthYear = block.First().Date.ToString("MM/yyyy");

                        column.Item().Text($"Dãy: {block.Key}").FontSize(15).Bold();
                        column.Item().Text($"Tháng: {monthYear}").FontSize(15).Bold();
                    });

                    page.Content().Padding(10).Table(table =>
                    {
                        // Define Columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Date
                            columns.RelativeColumn(2); // Room
                            columns.RelativeColumn(2); // Electricity
                            columns.RelativeColumn(2); // Water
                            columns.RelativeColumn(2); // Garbage
                            columns.RelativeColumn(2); // Room Cost
                            columns.RelativeColumn(2); // Total
                            columns.RelativeColumn(2); // Note
                        });

                        // Add Table Header with styling
                        table.Header(header =>
                        {
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Ngày").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Phòng").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Điện").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Nước").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Rác").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Phòng").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Tổng").Bold().FontSize(14);
                            header.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("Ghi chú").Bold().FontSize(14);
                        });

                        foreach (var invoice in block)
                        {
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.Date:dd/MM}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.Room:N0}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.ElectricTotal:N0}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.WaterTotal:N0}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.GarbageCost:N0}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.RoomCost:N0}").FontSize(13);
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text($"{invoice.TotalCost:N0}").FontSize(13).Bold();
                            table.Cell().Border(1).AlignCenter().PaddingVertical(7).Text("").FontSize(13).Italic(); // Empty note for now
                        }

                        // Add Summary Row with border and spacing
                        table.Cell().ColumnSpan(1).BorderTop(1).AlignRight().PaddingVertical(7).Text("Tổng cộng").Bold().FontSize(14);
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text(""); // Empty Room Summary
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text($"{block.Sum(i => i.ElectricTotal):N0}").FontSize(14);
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text($"{block.Sum(i => i.WaterTotal):N0}").FontSize(14);
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text($"{block.Sum(i => i.GarbageCost):N0}").FontSize(14);
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text($"{block.Sum(i => i.RoomCost):N0}").FontSize(14);
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text($"{block.Sum(i => i.TotalCost):N0}").FontSize(14).Bold();
                        table.Cell().BorderTop(1).AlignCenter().PaddingVertical(7).Text("").FontSize(14).Italic(); // Empty note in summary
                    });
                });
            }
        }
    }
}