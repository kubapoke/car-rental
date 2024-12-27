using CarRentalAPI.DTOs.Combinations;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Enums;
using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface IRentRepository
    {
        public Task<List<CarIdRentDatesDto>> GetChosenCarActiveRentDatesAsync(string? brand, string? model, string? location);
        public Task CreateNewRentAsync(Rent rent);
        public Task<List<RentInfoDto>> GetRentInformationByStatusAsync(RentStatuses? status);
        public Task<Rent?> GetRentOrNullByIdAsync(int id);
        public Task SaveRepositoryChangesAsync();
    }
}
