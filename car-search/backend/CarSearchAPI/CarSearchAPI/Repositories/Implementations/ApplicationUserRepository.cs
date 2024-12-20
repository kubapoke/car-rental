using CarSearchAPI.Abstractions;
using CarSearchAPI.Models;
using CarSearchAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarSearchAPI.Repositories.Implementations
{
    public class ApplicationUserRepository : IUserRepository
    {
        private readonly CarSearchDbContext _context;

        public ApplicationUserRepository(CarSearchDbContext context)
        {
            _context = context;
        }

        public async Task AddApplicationUserAsync(ApplicationUser applicationUser)
        {
            _context.applicationUsers.Add(applicationUser);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser?> GetApplicationUserOrNullByEmailAsync(string email)
        {
            return await _context.applicationUsers.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
