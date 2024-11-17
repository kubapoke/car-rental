namespace CarRentalAPI.DTOs.CarSearch
{
    public class NewSearchRentDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
