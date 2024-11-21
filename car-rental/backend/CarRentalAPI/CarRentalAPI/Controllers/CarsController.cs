using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarRentalDbContext _context;

        public CarsController(CarRentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("car-list")]
        public async Task<IActionResult> CarList()
        {
            // Get all cars from the database with related Model and Brand data
            var carList = await _context.Cars
                .Include(car => car.Model)
                .ThenInclude(model => model.Brand)
                .ToListAsync();

            // Group by Model and Brand, check if any car is active for each model
            var distinctCarModels = carList
                .GroupBy(car => new { ModelName = car.Model.Name, BrandName = car.Model.Brand.Name, Location = car.Location })  // Group by Model and Brand Name
                .Select(group => new 
                {
                    ModelName = group.Key.ModelName,
                    BrandName = group.Key.BrandName,
                    Location = group.Key.Location,
                    IsActive = group.Any(car => car.IsActive) // True if any car of this model is active in this location
                })
                .ToList();

            return Ok(distinctCarModels);
        }
    }
}