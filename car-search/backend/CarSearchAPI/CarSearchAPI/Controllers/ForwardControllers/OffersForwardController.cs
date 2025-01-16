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
using IExternalDataProvider = CarSearchAPI.Abstractions.IExternalDataProvider;

namespace CarSearchAPI.Controllers.ForwardControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersForwardController : ControllerBase
    {
        // this contains all of the external data providers
        private readonly IOfferPageService _offerPageService;

        public OffersForwardController(IOfferPageService offerPageService)
        {
            _offerPageService = offerPageService;
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
            // initialization
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            (int _page, int _pageSize) = _offerPageService.GetPageAndPageSize(page, pageSize);

            var offerAmountParametersDto = _offerPageService.GetGetOfferAmountParameters
                (brand, model, startDate, endDate, location, email);

            var offerListParametersDto = _offerPageService.GetGetOfferListParameters
                (brand, model, startDate, endDate, location, email, _pageSize);

            var offerPage = await _offerPageService.GetOfferPageAsync
                (offerAmountParametersDto, offerListParametersDto, _page, _pageSize);
            
            return Ok(offerPage);
        }

    }
}

