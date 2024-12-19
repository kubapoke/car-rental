namespace CarRentalAPI.DTOs.Rents
{
    public class CloseRentDto
    {
        public int Id { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }   
}
