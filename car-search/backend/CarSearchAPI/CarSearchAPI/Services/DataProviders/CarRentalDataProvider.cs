using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.ForwardingParameters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

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
                { "location", parameters.Location }
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
    }
}
