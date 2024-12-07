using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Abstractions.Repositories
{
    public interface IRentRepository
    {
        public Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDates(string? brand, string? model, string? location);
    }
}
