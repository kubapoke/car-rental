namespace CarSearchAPI.DTOs.DriveEasy
{
    public class DriveEasyCarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Photo  { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DriveEasyLocationDto Location { get; set; }
    }
}
