using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Authentication;
using CarRentalAPI.Models;
using CarRentalAPI.Repositories.Abstractions;

namespace CarRentalAPI.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }
        
        public async Task<Manager?> GetManagerOrNullFromCredentials(UserNamePasswordDto credentials)
        {
            return await _managerRepository.GetManagerOrNullByUserNameAsync(credentials.UserName);
        }
    }
}
