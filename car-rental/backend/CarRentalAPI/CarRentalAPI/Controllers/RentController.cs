using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly CarRentalDbContext _context;

        public RentController(CarRentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("car-list")]
        public async Task<IActionResult> CarList()
        {
            var carList = await _context.Cars
                .GroupBy(car => car.Model)  // group by Model
                .Select(group => new 
                {
                    Model = group.Key,
                    Brand = group.Key.Brand,
                    IsActive = group.Any(car => car.IsActive) // is any car active from that model?
                    // should also return location
                })
                .ToListAsync();

            return Ok(carList);
        }
    }
}