using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Authentication;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IPasswordService _passwordService;
        private readonly ISessionTokenManager _sessionTokenManager;

        public AuthController(IManagerService managerService, IPasswordService passwordService, ISessionTokenManager sessionTokenManager)
        {
            _managerService = managerService;
            _passwordService = passwordService;
            _sessionTokenManager = sessionTokenManager;
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn([FromBody] UserNamePasswordDto credentials)
        {
            var manager = await _managerService.GetManagerOrNullFromCredentials(credentials);

            if (manager == null)
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            if (!_passwordService.VerifyPassword(credentials.Password, manager.PasswordHash, manager.Salt))
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            var sessionToken = _sessionTokenManager.GetSessionToken(manager.UserName);
            return Ok(new { jwtToken = sessionToken });
        }

        [HttpPost("get-hash-salt")]
        public async Task<IActionResult> GetHashSalt([FromBody]string password)
        {
            (string hash, string salt) = _passwordService.HashPassword(password);
            return Ok(new {hash, salt});
        }
    }
}
