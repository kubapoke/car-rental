using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.DriveEasy;
using CarSearchAPI.DTOs.Users;
using Newtonsoft.Json;

namespace CarSearchAPI.Services.ProviderServices.ProviderCarServices;

public class DriveEasyProviderCarService : IProviderCarService
{
    private const string CarListEndpoint = "/api/Car";
    
    public async Task<List<CarDto>> GetCarListAsync(HttpClient client, string url)
    {
        url += CarListEndpoint;
        
        var response = await client.GetAsync(url);
            
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var driveEasyCars = JsonConvert.DeserializeObject<List<DriveEasyCarDto>>(responseContent);

            var cars = driveEasyCars.Select(r => new CarDto
            {
                BrandName = r.Brand,
                ModelName = r.Model,
                IsActive = r.Status == "Available",
                Location = r.Location.Name,
            }).ToList();
            
            return cars;
        }
        else
        {
            throw new Exception($"Error fetching data from \"/api/Cars/car-list\" at Drive Easy API");
        }
    }
    
    public string GetProviderName()
    {
        return "DriveEasy";
    }
}