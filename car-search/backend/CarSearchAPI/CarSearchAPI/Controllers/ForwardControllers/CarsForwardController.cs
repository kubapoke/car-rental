using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CarSearchAPI.Controllers.ForwardControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsForwardController : ControllerBase
    {
        // this contains all of the external data providers
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;

        public CarsForwardController(IEnumerable<IExternalDataProvider> dataProviders)
        {
            _dataProviders = dataProviders;
        }

        [HttpGet("car-list")]
        public async Task<ActionResult<IEnumerable<string>>> CarList()
        {
            var parameters = new Dictionary<string, string>();

            var aggregateCars = new List<CarDto>();

            foreach (var provider in _dataProviders)
            {
                try
                {
                    var cars = await provider.GetCarListAsync();
                    
                    aggregateCars.AddRange(cars);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data from provider: {ex.Message}");
                }
            }
            
            return Ok(aggregateCars);
        }
    }
}

