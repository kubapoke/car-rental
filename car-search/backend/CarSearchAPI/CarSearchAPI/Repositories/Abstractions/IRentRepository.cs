using CarSearchAPI.Models;

namespace CarSearchAPI.Repositories.Abstractions
{
    public interface IRentRepository
    {
        public Task<List<Rent>> GetUserRentsByEmailAsync(string email);
        public Task<Rent?> GetRentOrNullByIdAsync(int id);
        public Task AddNewRentAsync(Rent rent);
    }
}
