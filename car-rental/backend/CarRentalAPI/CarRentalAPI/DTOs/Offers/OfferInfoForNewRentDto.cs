namespace CarRentalAPI.DTOs.Offers
{
    public class OfferInfoForNewRentDto
    {
        public int CarId { get; set; }
        public decimal Price { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
