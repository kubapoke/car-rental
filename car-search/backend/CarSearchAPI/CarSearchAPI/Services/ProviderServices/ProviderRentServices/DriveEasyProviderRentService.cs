using System.Security.Claims;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.DriveEasy;
using CarSearchAPI.DTOs.Rents;

namespace CarSearchAPI.Services.ProviderServices.ProviderRentServices
{
    public class DriveEasyProviderRentService : IProviderRentService
    {
        private const string NewRentEndpoint = "/api/Offer/rentCar/";
        private const string ReadyToReturnEndpoint = "/api/Rent/readyToReturn/";
        
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        public async Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url, ClaimsPrincipal claimsPrincipal)
        {
            string offerId = claimsPrincipal.FindFirst("OfferId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;

            url = $"{url}{NewRentEndpoint}{offerId}/{CreateIdFromEmail(email)}";
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                NewDriveEasyRentDto newSearchRentDto = await response.Content.ReadFromJsonAsync<NewDriveEasyRentDto>();
                
                return new NewSearchRentDto()
                {
                    Brand = newSearchRentDto.CarBrand,
                    Model = newSearchRentDto.CarModel,
                    Email = email,
                    StartDate = newSearchRentDto.Start,
                    EndDate = newSearchRentDto.End,
                    RentalCompanyRentId = int.Parse(newSearchRentDto.RentId),
                };
            }

            var errorMessage = $"Error fetching data from \"/api/Offer/rentCar/\" at Drive Easy API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }

        public async Task<bool> SetRentStatusReadyToReturnAsync(HttpClient client, string url, int rentId)
        {
            url = $"{url}{ReadyToReturnEndpoint}{rentId}";
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorMessage = $"Error fetching data from \"/api/Rent/readyToReturn/\" at Drive Easy API. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            throw new HttpRequestException(errorMessage);
        }
        
        private string CreateIdFromEmail(string email)
        {
            return $"{email}-CAR-RENTAL";
        }
    }
}
