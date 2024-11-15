using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarSearchAPI.Controllers.ForwardControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersForwardController : ControllerBase
    {
        // this contains all of the external data providers
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;

        public OffersForwardController(IEnumerable<IExternalDataProvider> dataProviders)
        {
            _dataProviders = dataProviders;
        }
        
        [HttpGet("offer-list")]
        public async Task<IActionResult> OfferList(
        [FromQuery] string? brand,
        [FromQuery] string? model,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string? location)
        {
            var parameters = new Dictionary<string, string>
            {
                { "brand", brand ?? string.Empty },
                { "model", model ?? string.Empty },
                { "startDate", startDate.ToString("yyyy-MM-dd") },
                { "endDate", endDate.ToString("yyyy-MM-dd") },
                { "location", location ?? string.Empty }
            };

            var aggregateOffers = new List<OfferDto>();

            foreach (var provider in _dataProviders)
            {
                try
                {
                    var jsonData = await provider.GetDataAsync("Offers/offer-list", parameters);
                    
                    var offers = JsonConvert.DeserializeObject<List<OfferDto>>(jsonData);
                    aggregateOffers.AddRange(offers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data from provider: {ex.Message}");
                }
            }
            
            return Ok(aggregateOffers);
        }
    }
}

