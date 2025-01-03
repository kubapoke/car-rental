using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{   
    public interface ICarLookupService
    {
        public Task<Car?> GetCarOrNullFromOfferAsync(CachedOfferDto offer);
    }
}