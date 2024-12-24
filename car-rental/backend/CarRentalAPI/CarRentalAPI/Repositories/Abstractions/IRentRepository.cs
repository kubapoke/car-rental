using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface IRentRepository
    {
        public Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDatesAsync(string? brand, string? model, string? location);
        public Task CreateNewRentAsync(Rent rent);
    }
}
