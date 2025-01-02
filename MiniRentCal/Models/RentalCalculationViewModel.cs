using System.ComponentModel.DataAnnotations;

namespace MiniRentCal.Models
{
    public class RentalCalculationViewModel
    {
        public int UnitId { get; set; }
        public int RoomNumber { get; set; }
        public decimal UnitCost { get; set; }
        public decimal CurrentElectricAmount { get; set; }
        public decimal CurrentWaterAmount { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Không thể nhập số âm trong mục này")]
        public decimal NewElectricAmount { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Không thể nhập số âm trong mục này")]
        public decimal NewWaterAmount { get; set; }
        public decimal ElectricCost { get; set; }
        public decimal WaterCost { get; set; }
        public decimal GarbageCost { get; set; }

        public decimal TotalCost =>
            UnitCost +
            ((NewElectricAmount - CurrentElectricAmount) * ElectricCost) +
            ((NewWaterAmount - CurrentWaterAmount) * WaterCost) +
            GarbageCost;
        public DateTime CalculationDate { get; set; }
    }

}
