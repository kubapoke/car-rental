using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.ForwardingParameters;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Exception = System.Exception;

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
        
        [Authorize(Policy = "LegitUser")]
        [HttpGet("offer-list")]
        public async Task<IActionResult> OfferList(
        [FromQuery] string? brand,
        [FromQuery] string? model,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string? location,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
        {

            var parameters = new GetOfferListParametersDto()
            {
                Brand = brand,
                Model = model,
                StartDate = startDate,
                EndDate = endDate,
                Location = location,
                Email = User.FindFirst(ClaimTypes.Email)?.Value
            };

            var aggregateOffers = new List<OfferDto>();

            foreach (var provider in _dataProviders)
            {
                try
                {
                    var offers = await provider.GetOfferListAsync(parameters);
                    
                    aggregateOffers.AddRange(offers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data from provider: {ex.Message}");
                }
            }
            
            // handle paging logic
            if (pageSize is > 0 || page is >= 0)
            {
                int pageInt, pageSizeInt;

                if (page is null or < 0)
                    pageInt = 0;
                else
                    pageInt = (int)page;

                if (pageSize is null or <= 0)
                    pageSizeInt = 6;
                else
                    pageSizeInt = (int)pageSize;
                
                int startIndex = pageInt * pageSizeInt, count = pageSizeInt;

                if (startIndex < aggregateOffers.Count)
                {
                    if(count > aggregateOffers.Count - startIndex)
                        count = aggregateOffers.Count - startIndex;
                    
                    aggregateOffers = aggregateOffers.GetRange(startIndex, count);
                }
            }
            
            return Ok(aggregateOffers);
        }
    }
}

