using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalAPI.Controllers
{
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
           [FromQuery] string? location)
       {
           if (startDate > endDate)
           {
               return BadRequest("Start date must be earlier than end date.");
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
                   Id = group.CarId,
                   Price = _priceGenerator.GeneratePrice(group.Model.BasePrice, startDate, endDate),
                   Conditions = Conditions,
                   CompanyName = CompanyName,
                   StartDate = startDate,
                   EndDate = endDate,
               })
               .ToList();
           
           return Ok(offers);
       }
   } 
}

