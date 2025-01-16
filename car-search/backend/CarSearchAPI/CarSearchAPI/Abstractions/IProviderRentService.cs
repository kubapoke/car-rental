using System.Security.Claims;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.Rents;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderRentService : IProviderService
    {
        public Task<NewSearchRentDto> CreateNewRentAsync(HttpClient client, string url, ClaimsPrincipal claimsPrincipal);
        public Task<bool> SetRentStatusReadyToReturnAsync(HttpClient client, string url, string rentId);
    }
}