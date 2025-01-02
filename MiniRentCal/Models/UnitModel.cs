using System.ComponentModel.DataAnnotations;

namespace MiniRentCal.Models
{
    public class UnitModel
    {
        [Key]
        public required int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        [Required]
        [Display(Name = "Block Number")]
        public required string BlockNumber { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Required]
        [Display(Name = "Current Electric Amount")]
        public decimal CurrentElectricAmount { get; set; }

        [Required]
        [Display(Name = "Current Water Amount")]
        public decimal CurrentWaterAmount { get; set; }
    }
}
