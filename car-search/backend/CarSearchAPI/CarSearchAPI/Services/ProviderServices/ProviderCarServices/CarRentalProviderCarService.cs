using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.Users;
using Newtonsoft.Json;

namespace CarSearchAPI.Services.ProviderServices.ProviderCarServices
{
    public class CarRentalProviderCarService : IProviderCarService
    {
        private const string CarListEndpoint = "/api/Cars/car-list";
        
        public async Task<List<CarDto>> GetCarListAsync(HttpClient client, string url)
        {
            url += CarListEndpoint;
            
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<List<CarDto>>(responseContent);
                return cars;
            }
            else
            {
                throw new Exception($"Error fetching data from \"/api/Cars/car-list\" at Car Rental API");
            }
        }

        public string GetProviderName()
        {
            return "CarRental";
        }
    }
}

