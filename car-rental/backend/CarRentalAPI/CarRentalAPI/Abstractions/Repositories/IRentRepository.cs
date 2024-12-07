using CarRentalAPI.DTOs.Combinations;

namespace CarRentalAPI.Abstractions.Repositories
{
    public interface IRentRepository
    {
        public Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDates(string? brand, string? model, DateTime startDate, DateTime endDate, string? location);
    }
}
