using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface IRentRepository
    {
        public Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDatesAsync(string? brand, string? model, string? location);
    }
}
