using System.Security.Claims;
using CarSearchAPI.DTOs.CarSearch;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderRentService
    {
        public Task<NewSearchRentDto> CreateNewRentAsync(ClaimsPrincipal claimsPrincipal);
        public Task<bool> SetRentStatusReadyToReturnAsync(int rentId);
    }
}