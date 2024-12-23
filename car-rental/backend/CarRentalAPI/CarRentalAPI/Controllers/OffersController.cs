using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Controllers
{
   [Authorize(Policy = "Backend")]
   [Route("api/[controller]")]
   [ApiController]
   public class OffersController : ControllerBase
   {
       private readonly IOfferService _offerService;
       private const string Conditions = "{}";
       private const string CompanyName = "CarRental";
   
       public OffersController(IOfferService offerService)
       {
           _offerService = offerService;
       }

       [HttpGet("offer-amount")]
       public async Task<IActionResult> OfferAmount(
           [FromQuery] string? brand,
           [FromQuery] string? model,
           [FromQuery] DateTime startDate,
           [FromQuery] DateTime endDate,
           [FromQuery] string? location)
       {
           if (startDate > endDate)
           {
               return BadRequest("Start date must be earlier than end date.");
           }
           if (startDate.Date < DateTime.Now.Date)
           {
               return BadRequest("Start date must be in the future.");
           }
           
           var offerCount = await _offerService.GetOffersCountAsync(brand, model, startDate, endDate, location);
           return Ok(offerCount);
       }
   
       [HttpGet("offer-list")]
       public async Task<IActionResult> OfferList(
           [FromQuery] string? brand,
           [FromQuery] string? model,
           [FromQuery] DateTime startDate,
           [FromQuery] DateTime endDate,
           [FromQuery] string? location,
           [FromQuery] string? email,
           [FromQuery] int? page,
           [FromQuery] int? pageSize)
       {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }
            if (startDate.Date < DateTime.Now.Date)
            {
                return BadRequest("Start date must be in the future.");
            }
            if (email == null)
            {
                return BadRequest("User email is required.");
            }

            var offers = await _offerService.GetNewOffersAsync(brand, model, startDate, endDate, location, 
                email, Conditions, CompanyName, page, pageSize);

            if (offers.Count == 0)
            {
                return BadRequest("We are sorry. There are no available cars of this type.");
            }

            return Ok(offers);
        }
   } 
}

