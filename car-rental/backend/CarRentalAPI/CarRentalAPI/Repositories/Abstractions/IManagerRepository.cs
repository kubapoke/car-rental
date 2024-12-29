using CarRentalAPI.Models;

namespace CarRentalAPI.Repositories.Abstractions
{
    public interface IManagerRepository
    {
        public Task<Manager?> GetManagerOrNullByUserNameAsync(string userName);
    }
}