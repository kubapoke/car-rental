namespace CarRentalAPI.DTOs.Offers
{
    public class OfferInfoForNewRent
    {
        public int CarId { get; set; }
        public float Price { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
