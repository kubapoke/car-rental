using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface ICarRepository
    {
        public Task<List<Car>> GetCarsByIdAsync(List<int> ids, string? brand, string? model, string? location);
    }
}
