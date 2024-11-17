namespace CarRentalAPI.DTOs.Offers
{
    public class OfferInfoForNewRentDto
    {
        public int CarId { get; set; }
        public decimal Price { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
