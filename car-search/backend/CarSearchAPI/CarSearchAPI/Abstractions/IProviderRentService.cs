using System.Security.Claims;
using CarSearchAPI.DTOs.CarSearch;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderRentService
    {
        public Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url, StringContent? jsonContent);
        public Task<bool> SetRentStatusReadyToReturnAsync(HttpClient client, string url, StringContent? jsonContent);
    }
}