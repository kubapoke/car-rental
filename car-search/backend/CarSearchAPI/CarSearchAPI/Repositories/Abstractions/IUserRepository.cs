using CarSearchAPI.Models;

namespace CarSearchAPI.Repositories.Abstractions
{
    public interface IUserRepository
    {
        public Task<ApplicationUser?> GetApplicationUSerByEmailAsync(string email);
        public Task AddApplicationUserAsync(ApplicationUser applicationUser);
    }
}
