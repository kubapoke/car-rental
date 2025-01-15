using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarSearch;

namespace CarSearchAPI.Services.ProviderServices.ProviderRentServices
{
    public class DriveEasyProviderRentService : IProviderRentService
    {
        public string GetProviderName()
        {
            return "DriveEasy";
        }

        public Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url, StringContent? jsonContent)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetRentStatusReadyToReturnAsync(HttpClient client, string url, StringContent? jsonContent)
        {
            throw new NotImplementedException();
        }
    }
}
