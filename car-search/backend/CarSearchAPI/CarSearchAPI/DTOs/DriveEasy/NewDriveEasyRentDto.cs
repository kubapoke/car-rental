namespace CarSearchAPI.DTOs.DriveEasy
{
    public class NewDriveEasyRentDto
    {
        public string RentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }
        public decimal CarPrice { get; set; }
    }
}
