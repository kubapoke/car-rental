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
        private readonly OffersService _offersService;
       private const string Conditions = "{}";
       private const string CompanyName = "CarRental";
   
       public OffersController(OffersService offersService)
       {
           _offersService = offersService;
       }
   
       [HttpGet("offer-list")]
       public async Task<IActionResult> OfferList(
           [FromQuery] string? brand,
           [FromQuery] string? model,
           [FromQuery] DateTime startDate,
           [FromQuery] DateTime endDate,
           [FromQuery] string? location,
           [FromQuery] string? email)
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

            List<OfferForCarSearchDto> offers = await _offersService.GetNewOffers(brand, model, startDate, endDate, location, email, Conditions, CompanyName);

            if (offers.Count == 0)
            {
                return BadRequest("We are sorry. There are no available cars of this type.");
            }

            return Ok(offers);
        }
   } 
}

