namespace CarSearchAPI.DTOs.DriveEasy
{
    public class DriveEasyOfferDto
    {
        public string Id { get; set; }
        public DriveEasyCarDto Car { get; set; }
        public string ClientId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
