using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface IRentService
    {
        public Task<NewSearchRentDto> CreateAndGetNewRentAsync(CachedOfferDto offer, string userEmail);
    }   
}