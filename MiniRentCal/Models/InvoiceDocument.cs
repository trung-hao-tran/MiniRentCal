namespace MiniRentCal.Models
{
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using System.Collections.Generic;

    public class InvoiceDocument : IDocument
    {
        private readonly List<InvoiceData> _invoices;

        public InvoiceDocument(List<InvoiceData> invoices)
        {
            _invoices = invoices;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        [Obsolete]
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
            page.Size(PageSizes.A4); // Standard A4 page size
            page.Margin(4);        // Outer margin for cutting
            page.Content().Padding(1).Grid(grid =>
            {
            grid.Columns(2);           // Two invoices per row
            grid.VerticalSpacing(6);  // Space between rows
            grid.HorizontalSpacing(6); // Space between columns

            foreach (var invoice in _invoices)
            {
                grid.Item().Border(1).Padding(6).Height(380).Grid(innerGrid =>
                {
                    innerGrid.Columns(4); // Three-column grid layout

                    // Room and Date on the same row
                    innerGrid.Item(2).AlignLeft().Text($"Phòng: {invoice.Room}").FontSize(15).Bold();
                    innerGrid.Item(2).AlignCenter().Text($"Ngày: {invoice.Date:dd/MM/yyyy}").FontSize(13);

                    innerGrid.Spacing(6);

                    // Electric Section
                    innerGrid.Item(4).PaddingLeft(10).Text("⭐ ĐIỆN:").FontSize(15).Bold();

                    innerGrid.Spacing(6);

                    innerGrid.Item(1).PaddingLeft(13).AlignLeft().Text("Số cũ:").FontSize(13);
                    innerGrid.Item(1).PaddingLeft(13).AlignRight().Text($"{invoice.OldElectricAmount:N0}").FontSize(13);
                    innerGrid.Item(2);

                    innerGrid.Item(1).PaddingLeft(13).AlignLeft().Text("Số mới:").FontSize(13);
                    innerGrid.Item(1).PaddingLeft(13).AlignRight().Text($"{invoice.NewElectricAmount:N0}").FontSize(13);
                    innerGrid.Item(2);

                    innerGrid.Item(1);
                    innerGrid.Item(1).AlignRight().Text($"{invoice.ElectricDifference:N0}").FontSize(13);
                    innerGrid.Item(1).AlignCenter().Text($"x {invoice.ElectricCost * 1000:N0} =").FontSize(13);
                    innerGrid.Item(1).AlignCenter().Text($" {invoice.ElectricTotal * 1000:N0}").FontSize(13);

                    innerGrid.Spacing(6);

                    // Water Section
                    innerGrid.Item(4).PaddingLeft(10).Text("⭐ NƯỚC:").FontSize(15).Bold();

                    innerGrid.Spacing(6);
                    innerGrid.Item(1).PaddingLeft(13).AlignLeft().Text("Số cũ:").FontSize(13);
                    innerGrid.Item(1).PaddingLeft(13).AlignRight().Text($"{invoice.OldWaterAmount:N0}").FontSize(13);
                    innerGrid.Item(2);

                    innerGrid.Item(1).PaddingLeft(13).AlignLeft().Text("Số mới:").FontSize(13);
                    innerGrid.Item(1).PaddingLeft(13).AlignRight().Text($"{invoice.NewWaterAmount:N0}").FontSize(13);
                    innerGrid.Item(2);

                    innerGrid.Item(1);
                    innerGrid.Item(1).AlignRight().Text($"{invoice.WaterDifference:N0}").FontSize(13);
                    innerGrid.Item(1).AlignCenter().Text($"x {invoice.WaterCost * 1000:N0} =").FontSize(13);
                    innerGrid.Item(1).AlignCenter().Text($" {invoice.WaterTotal * 1000:N0}").FontSize(13);
                    innerGrid.Spacing(6);
                    // Room Cost
                    innerGrid.Item(3).PaddingLeft(10).AlignLeft().Text("⭐ PHÒNG:").FontSize(15).Bold();
                    innerGrid.Item(1).AlignCenter().Text($"{invoice.RoomCost * 1000:N0}").FontSize(13);
                    innerGrid.Spacing(6);
                    // Garbage Cost
                    innerGrid.Item(3).PaddingLeft(10).AlignLeft().Text("⭐ RÁC:").FontSize(15).Bold();
                    innerGrid.Item(1).AlignCenter().Text($"{invoice.GarbageCost * 1000:N0}").FontSize(13);
                    innerGrid.Spacing(6);
                    // Total Cost
                    innerGrid.Item(3).PaddingLeft(10).AlignLeft().Text("⭐ TỔNG CỘNG:").FontSize(15).Bold();
                    innerGrid.Item(1).AlignCenter().Text($"{invoice.TotalCost * 1000:N0}").FontSize(13).Bold();
                    innerGrid.Spacing(8);
                    // Footer notes (free from grid)
                    innerGrid.Item(4).AlignBottom().Text("Trả phòng vui lòng báo trước 1 tháng.").FontSize(13).Italic();

                    innerGrid.Item(4).AlignBottom().Text("Nếu không báo, nhà trọ không hoàn cọc.").FontSize(13).Italic();

                    innerGrid.Item(4).AlignBottom().Text("TK: ACB 999 85 86 NGUYỄN MINH HOÀNG").FontSize(13).Bold();
                });
                }
            });
            });
        }
    }
}
