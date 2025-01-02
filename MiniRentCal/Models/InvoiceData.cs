namespace MiniRentCal.Models
{
    public class InvoiceData
    {
        public string Room => $"{BlockNumber}{RoomNumber}";
        public string BlockNumber { get; set; }
        public int  RoomNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal NewElectricAmount { get; set; }
        public decimal OldElectricAmount { get; set; }
        public decimal ElectricDifference => NewElectricAmount - OldElectricAmount;
        public decimal ElectricCost { get; set; }
        public decimal ElectricTotal => ElectricDifference * ElectricCost;

        public decimal NewWaterAmount { get; set; }
        public decimal OldWaterAmount { get; set; }
        public decimal WaterDifference => NewWaterAmount - OldWaterAmount;
        public decimal WaterCost { get; set; }
        public decimal WaterTotal => WaterDifference * WaterCost;

        public decimal GarbageCost { get; set; }
        public decimal RoomCost { get; set; }
        public decimal TotalCost => ElectricTotal + WaterTotal + GarbageCost + RoomCost;
    }
}
