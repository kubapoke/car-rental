using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface ICarService
    {
        public Task<List<Car>> GetAllCarsAsync();
        public Task<List<CarInfoDto>> GetAllDistinctCarTypesAsync();
        public Task<Car?> GetCarOrNullFromOfferAsync(CachedOfferDto offer);
    }
}