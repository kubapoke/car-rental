using Microsoft.Data.SqlClient.DataClassification;

namespace CarSearchAPI.DTOs.CarRental
{
    public class NewRentalRentDto
    {
        public int CarId { get; set; }
        public string Email { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
