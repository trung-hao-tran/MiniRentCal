using System.ComponentModel.DataAnnotations;

namespace MiniRentCal.Models
{
    public class UtilityModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Electric Cost")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Electric cost must be a positive value")]
        public decimal ElectricCost { get; set; }

        [Required]
        [Display(Name = "Water Cost")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Water cost must be a positive value")]
        public decimal WaterCost { get; set; }

        [Required]
        [Display(Name = "Garbage Cost")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Garbage cost must be a positive value")]
        public decimal GarbageCost { get; set; }

    }
}
