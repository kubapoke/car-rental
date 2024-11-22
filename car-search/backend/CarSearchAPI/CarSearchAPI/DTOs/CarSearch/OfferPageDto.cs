using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.DTOs.CarSearch;

public class OfferPageDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public int TotalOffers { get; set; }
    public List<OfferDto> Offers { get; set; }
}