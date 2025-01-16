using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.Rents;

namespace CarSearchAPI.Services.ProviderServices.ProviderRentServices
{
    public class CarRentalProviderRentService : IProviderRentService
    {
        private const string NewRentEndpoint = "/api/Rents/create-new-rent";
        private const string ReadyToReturnEndpoint = "/api/Rents/set-rent-status-ready-to-return";
        
        public async Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url,
            ClaimsPrincipal claimsPrincipal)
        {
            url += NewRentEndpoint;
            
            string offerId = claimsPrincipal.FindFirst("OfferId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;

            NewRentalRentDto newRentDto = new NewRentalRentDto
            {
                OfferId = offerId,
                Email = email,
            };
            
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newRentDto),
                Encoding.UTF8,
                "application/json"
            );
            
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
            int rentId)
        {
            url += ReadyToReturnEndpoint;
            
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(rentId),
                Encoding.UTF8,
                "application/json"
            );
            
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
