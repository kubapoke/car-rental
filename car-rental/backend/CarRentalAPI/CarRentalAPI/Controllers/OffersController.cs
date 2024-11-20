using CarRentalAPI.Abstractions;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Controllers
{
   [Authorize]
   [Route("api/[controller]")]
   [ApiController]
   public class OffersController : ControllerBase
   {
       private readonly CarRentalDbContext _context;
       private readonly IPriceGenerator _priceGenerator;
       private const string Conditions = "{}";
       private const string CompanyName = "CarRental";
   
       public OffersController(CarRentalDbContext context, IPriceGenerator priceGenerator)
       {
           _context = context;
           _priceGenerator = priceGenerator;
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
   
           var carList = await _context.Cars
               .Include(car => car.Model)
               .ThenInclude(model => model.Brand)
               .ToListAsync();
   
           var offers = carList
               .Where(group =>
                   (group.IsActive)
                   && (brand.IsNullOrEmpty() || group.Model.Brand.Name == brand)
                   && (model.IsNullOrEmpty() || group.Model.Name == model)
                   && (location.IsNullOrEmpty() || group.Location == location))
               .Select(group => new
               {
                   CarId = group.CarId,
                   Brand = group.Model.Brand.Name,
                   Model = group.Model.Year == null ? group.Model.Name : group.Model.Name + " " + group.Model.Year,
                   Price = _priceGenerator.GeneratePrice(group.Model.BasePrice, startDate, endDate),
                   Conditions = Conditions,
                   CompanyName = CompanyName,
                   Location = group.Location,
                   StartDate = startDate,
                   EndDate = endDate,
                   Email = email
               })
               .ToList();
           
           return Ok(offers);
       }
   } 
}

