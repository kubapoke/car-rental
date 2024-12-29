using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface ICarRepository
    {
        public Task<List<Car>> GetAllCarsAsync();
        public Task<List<Car>> GetCarsByIdAndFiltersAsync(List<int> ids, string? brand, string? model, string? location);
        public Task<Car?> GetCarOrNullByIdAsync(int id);
    }
}
