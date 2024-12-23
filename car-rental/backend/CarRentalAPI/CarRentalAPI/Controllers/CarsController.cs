using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Controller
{
    [Authorize(Policy = "Backend")]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("car-list")]
        public async Task<IActionResult> CarList()
        {
            var distinctCarModels = await _carService.GetAllDistinctCarTypesAsync();

            return Ok(distinctCarModels);
        }
    }
}