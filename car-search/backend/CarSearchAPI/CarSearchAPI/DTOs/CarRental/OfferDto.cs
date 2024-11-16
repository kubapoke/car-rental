namespace CarSearchAPI.DTOs.CarRental;

public class OfferDto
{
    public int CarId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Email { get; set; }
    public decimal Price { get; set; }
    public string Conditions { get; set; }
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}