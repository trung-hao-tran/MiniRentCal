namespace MiniRentCal.Models
{
    public class RentalBlockViewModel
    {
        public string BlockNumber { get; set; }
        public List<RentalCalculationViewModel> Units { get; set; }
    }
}
