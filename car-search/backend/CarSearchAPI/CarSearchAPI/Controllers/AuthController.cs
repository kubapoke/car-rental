using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarSearchAPI.Models;
using CarSearchAPI.Services;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using CarSearchAPI.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace CarSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarSearchDbContext _context;

        public AuthController(CarSearchDbContext context)
        {
            _context = context;
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] string idToken)
        {
            GoogleAuthService googleAuthService = new GoogleAuthService();
            SessionTokenManager sessionTokenManager = new SessionTokenManager();
            bool isValid = await googleAuthService.VerifyGoogleToken(idToken);

            if (!isValid)
            {
                return Unauthorized();
            }

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.email == payload.Email);

            if (user != null)
            {
                var jwtToken = sessionTokenManager.GenerateJwtToken(user.email, false); 
                return Ok(new { jwtToken, isNewUser = false });
            }
            else
            {
                var tmpToken = sessionTokenManager.GenerateJwtToken(payload.Email, true);
                return Ok(new { tmpToken, isNewUser = true });
            }
        }

        [Authorize]
        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] NewUserInfoDto userInfo)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid temporary session");
            }
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    email = email,
                    name = userInfo.Name,
                    surname = userInfo.Surname,
                    birthDate = userInfo.birthDate,
                    licenceDate = userInfo.licenceDate
                };
                _context.applicationUsers.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("This user already exist");
            }
            SessionTokenManager sessionTokenManager = new SessionTokenManager();
            var sessionToken = sessionTokenManager.GenerateJwtToken(email, false);
            return Ok(new {sessionToken});
        }
    }


}
