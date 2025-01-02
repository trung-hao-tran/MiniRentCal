namespace MiniRentCal.Models
{
    public class CalculationSession
    {
        public int Id { get; set; } // Primary Key
        public DateTime CalculationDate { get; set; } // Date of calculation
        public decimal TotalCost { get; set; } // Total cost for the session
        public int TotalUnits { get; set; } // Total number of units in the session
        public string SerializedUnits { get; set; } // JSON representation of all unit details
    }
}
