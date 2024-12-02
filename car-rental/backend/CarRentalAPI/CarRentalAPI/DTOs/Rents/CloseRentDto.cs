namespace CarRentalAPI.DTOs.Rents
{
    public class CloseRentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }

    }
}
