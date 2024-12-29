 using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface ICarTypeService
    {
        public Task<List<CarInfoDto>> GetAllDistinctCarTypesAsync();
    }
}