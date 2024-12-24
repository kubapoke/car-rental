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
            var tasks = _dataProviders.Select(async provider =>
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                try
                {                    
                    return await provider.GetCarListAsync(cts.Token);
                }
                catch (TaskCanceledException ex)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Request to {provider.GetProviderName()} was canceled because the cancellation token was triggered.");
                    }
                    else
                    {
                        Console.WriteLine($"Request to {provider.GetProviderName()} timed out or was canceled for another reason.");
                    }
                    return new List<CarDto>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data from to {provider.GetProviderName()}: {ex.Message}");
                    return new List<CarDto>();
                }
            });

            var results = await Task.WhenAll(tasks);
            var aggregateCars = results.SelectMany(c => c).ToList();

            return Ok(aggregateCars);
        }
    }
}

