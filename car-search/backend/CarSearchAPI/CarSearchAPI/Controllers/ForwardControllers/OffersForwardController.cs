using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
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
            var offerPage = new OfferPageDto();

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
            
            int pageInt = Math.Max(page ?? 0, 0);
            int pageSizeInt = Math.Max(pageSize ?? 6, 1);

            if (page == null && pageSize == null)
            {
                pageInt = 0;
                pageSizeInt = aggregateOffers.Count;
            }
            
            offerPage.TotalOffers = aggregateOffers.Count;
            offerPage.PageCount = Math.Max(1, (int)Math.Ceiling((double)aggregateOffers.Count / pageSizeInt));
            
            // apply paging
            aggregateOffers = aggregateOffers
                .Skip(pageInt * pageSizeInt)
                .Take(pageSizeInt)
                .ToList();
            
            offerPage.Page = pageInt;
            offerPage.PageSize = aggregateOffers.Count;
            offerPage.Offers = aggregateOffers;
            
            return Ok(offerPage);
        }
    }
}

