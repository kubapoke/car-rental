using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.DTOs.CarSearch;

public class OfferPageDto
{
    public int PageCount { get; set; }
    public List<OfferDto> Offers { get; set; }
}