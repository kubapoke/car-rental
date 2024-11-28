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
        private readonly CarRentalDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly SessionTokenManager _sessionTokenManager;

        public AuthController(CarRentalDbContext context, PasswordHasher passwordHasher, SessionTokenManager sessionTokenManager)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _sessionTokenManager = sessionTokenManager;
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn([FromBody] UserNamePasswordDto credentials)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(manager => manager.UserName == credentials.UserName);

            if (manager == null)
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            if (!_passwordHasher.VerifyPassword(credentials.Password, manager.PasswordHash, manager.Salt))
            {
                return Unauthorized("Your provided wrong username or password!");
            }

            var jwtToken = _sessionTokenManager.GenerateJwtToken(manager.UserName);
            return Ok(jwtToken);
        }

        [HttpPost("get-hash-salt")]
        public async Task<IActionResult> GetHashSalt([FromBody]string password)
        {
            (string hash, string salt) = _passwordHasher.HashPassowrd(password);
            return Ok(new {hash, salt});
        }

        
    }
}
