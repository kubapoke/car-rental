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
        private readonly PasswordHasher _passwordHasher;
        private readonly SessionTokenManager _sessionTokenManager;
        private readonly IManagerService _managerService;

        public AuthController(PasswordHasher passwordHasher, SessionTokenManager sessionTokenManager, IManagerService managerService)
        {
            _passwordHasher = passwordHasher;
            _sessionTokenManager = sessionTokenManager;
            _managerService = managerService;
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn([FromBody] UserNamePasswordDto credentials)
        {
            var manager = await _managerService.GetManagerOrNullFromCredentials(credentials);

            if (manager == null)
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            if (!_passwordHasher.VerifyPassword(credentials.Password, manager.PasswordHash, manager.Salt))
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            var jwtToken = _sessionTokenManager.GenerateJwtToken(manager.UserName);
            return Ok(new { jwtToken });
        }

        [HttpPost("get-hash-salt")]
        public async Task<IActionResult> GetHashSalt([FromBody]string password)
        {
            (string hash, string salt) = _passwordHasher.HashPassowrd(password);
            return Ok(new {hash, salt});
        }
    }
}
