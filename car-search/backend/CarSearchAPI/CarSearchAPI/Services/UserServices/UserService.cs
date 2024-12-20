using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.Users;
using CarSearchAPI.Models;
using CarSearchAPI.Repositories.Abstractions;

namespace CarSearchAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ServeNewCreatedUser(string email, NewUserInfoDto userInfo)
        {
            var user = await _userRepository.GetApplicationUserOrNullByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    Name = userInfo.name,
                    Surname = userInfo.surname,
                    BirthDate = userInfo.birthDate,
                    LicenceDate = userInfo.licenceDate
                };

                await _userRepository.AddApplicationUserAsync(user);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
