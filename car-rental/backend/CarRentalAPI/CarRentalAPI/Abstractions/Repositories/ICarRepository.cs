using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions.Repositories
{
    public interface ICarRepository
    {
        public Task<List<Car>> GetCarsByIdAsync(List<int> ids, string? brand, string? model, string? location);
    }
}
