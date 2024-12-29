 using CarRentalAPI.DTOs.CarSearch;

namespace CarRentalAPI.Abstractions
{
    public interface ICarTypeService
    {
        public Task<List<CarInfoDto>> GetAllDistinctCarTypesAsync();
    }
}