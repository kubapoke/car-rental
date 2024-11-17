using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CarSearchAPI.Services.DataProviders
{
    public class CarRentalDataProvider : IExternalDataProvider
    {
        // This will get data from an appropriate endpoint (using FromQuery parameters!) from CarRentalAPI
        private readonly IHttpClientFactory _httpClientFactory;

        public CarRentalDataProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string GetProviderName()
        {
            return "CarRental";
        }

        public async Task<string> GetCarListAsync()
        {
            var client = _httpClientFactory.CreateClient();
            
            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var endpoint = "/api/Cars/car-list";

            var url = $"{carRentalApiUrl}{endpoint}";
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error fetching data from {endpoint} at Car Rental API");
            }
        }

        public async Task<string> GetOfferListAsync(GetOfferListParametersDto parameters)
        {
            var client = _httpClientFactory.CreateClient();
            
            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL");
            var endpoint = "/api/Offers/offer-list";
            
            var queryParameters = new Dictionary<string, string?>()
            {
                { "model", parameters.Model },
                { "brand", parameters.Brand },
                { "startDate", parameters.StartDate.ToString("o") },
                { "endDate", parameters.EndDate.ToString("o") },
                { "location", parameters.Location },
                { "email", parameters.Email },
            };
            
            // this creates an appropriate url
            var url = QueryHelpers.AddQueryString($"{carRentalApiUrl}{endpoint}", 
                queryParameters.Where(p => p.Value != null));
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error fetching data from {endpoint} at Car Rental API");
            }
        }

        public async Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal)
        {
            var client = _httpClientFactory.CreateClient();

            string carId = claimsPrincipal.FindFirst("CarId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;
            string price = claimsPrincipal.FindFirst("Price")?.Value;
            string startDate = claimsPrincipal.FindFirst("StartDate")?.Value;
            string endDate = claimsPrincipal.FindFirst("EndDate")?.Value;

            NewRentalRentDto newRentDto = new NewRentalRentDto
            {
                CarId = int.Parse(carId),
                Email = email,
                Price = decimal.Parse(price),
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate)
            };

            var carRentalApiUrl = Environment.GetEnvironmentVariable("CAR_RENTAL_API_URL")
                          ?? throw new InvalidOperationException("CAR_RENTAL_API_URL is not set.");
            const string endpoint = "/api/Rents/create-new-rent";

            Console.WriteLine($"Serialized Payload: {JsonSerializer.Serialize(newRentDto)}");

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newRentDto),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"{carRentalApiUrl}{endpoint}";

            url = "http://localhost:5237/api/Rents/create-new-rent";

            var response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return new NewSearchRentDto();
            }

            var errorMessage = $"Error fetching data from {endpoint} at Car Rental API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }
        
    }
}
