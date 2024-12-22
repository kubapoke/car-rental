using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories.Implementations
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly CarRentalDbContext _context;

        public ManagerRepository(CarRentalDbContext context)
        {
            _context = context;
        }
        
        public async Task<Manager?> GetManagerOrNullByUserNameAsync(string userName)
        {
            return await _context.Managers.FirstOrDefaultAsync(manager => manager.UserName == userName);
        }
    }
}
