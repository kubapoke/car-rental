using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarSearch;

namespace CarSearchAPI.Services.ProviderServices.ProviderRentServices
{
    public class CarRentalProviderRentService : IProviderRentService
    {
        public async Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url,
            StringContent? jsonContent)
        {
            var response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                NewSearchRentDto newSearchRentDto = await response.Content.ReadFromJsonAsync<NewSearchRentDto>();
                return newSearchRentDto;
            }

            var errorMessage = $"Error fetching data from \"/api/Rents/create-new-rent\" at Car Rental API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }

        public async Task<bool> SetRentStatusReadyToReturnAsync(HttpClient client, string url,
            StringContent? jsonContent)
        {
            var response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorMessage = $"Error fetching data from \"/api/Rents/set-rent-status-ready-to-return\" at Car Rental API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }
        
        public string GetProviderName()
        {
            return "CarRental";
        }
    }
}
