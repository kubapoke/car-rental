using CarSearchAPI.DTOs.Users;
using CarSearchAPI.Models;

namespace CarSearchAPI.Abstractions
{
    public interface IUserService
    {
        public Task<bool> ServeNewCreatedUser(string email, NewUserInfoDto userInfo);

    }
}
