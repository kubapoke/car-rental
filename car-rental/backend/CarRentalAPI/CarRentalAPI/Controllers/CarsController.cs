﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Controller
{
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
                .GroupBy(car => new { ModelName = car.Model.Name, BrandName = car.Model.Brand.Name })  // Group by Model and Brand Name
                .Select(group => new 
                {
                    ModelName = group.Key.ModelName,
                    BrandName = group.Key.BrandName,
                    IsActive = group.Any(car => car.IsActive) // True if any car of this model is active
                    // Should also return location
                })
                .ToList();

            return Ok(distinctCarModels);
        }
    }
}