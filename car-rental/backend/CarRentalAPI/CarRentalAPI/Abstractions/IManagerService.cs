using CarRentalAPI.DTOs.Authentication;
using CarRentalAPI.Models;

namespace CarRentalAPI.Abstractions
{
    public interface IManagerService
    {
        public Task<Manager?> GetManagerOrNullFromCredentials(UserNamePasswordDto credentials);
    }
}