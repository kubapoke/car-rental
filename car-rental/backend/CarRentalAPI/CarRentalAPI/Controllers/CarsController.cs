using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Authorize(Policy = "Backend")]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarTypeService _carTypeService;

        public CarsController(ICarTypeService carTypeService)
        {
            _carTypeService = carTypeService;
        }

        [HttpGet("car-list")]
        public async Task<IActionResult> CarList()
        {
            var distinctCarModels = await _carTypeService.GetAllDistinctCarTypesAsync();

            return Ok(distinctCarModels);
        }
    }
}