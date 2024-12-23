using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface ICarService
    {
        public Task<List<Car>> GetAllCarsAsync();
        public Task<List<CarInfoDto>> GetAllDistinctCarTypesAsync();
    }
}