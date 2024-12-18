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
using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarSearchDbContext _context;

        private readonly IAuthService _authService;

        private readonly SessionTokenManager _sessionTokenManager;

        public AuthController(CarSearchDbContext context, IAuthService authService, SessionTokenManager sessionTokenManager)
        {
            _context = context;
            _authService = authService;
            _sessionTokenManager = sessionTokenManager;
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] string idToken)
        {            
            bool isValid = await _authService.VerifyToken(idToken);

            if (!isValid)
            {
                return Unauthorized();
            }

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.Email == payload.Email); 

            if (user != null)
            {
                var jwtToken = _sessionTokenManager.GenerateJwtToken(user.Email, false);
                return Ok(new { jwtToken, isNewUser = false });
            }
            else
            {
                var jwtToken = _sessionTokenManager.GenerateJwtToken(payload.Email, true);
                return Ok(new { jwtToken, isNewUser = true });
            }
        }

        [Authorize(Policy = "ProtoUser")] 
        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] NewUserInfoDto userInfo)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid temporary session");
            }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.Email == email);
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

                _context.applicationUsers.Add(user); 
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("This user already exist");
            }

            var sessionToken = _sessionTokenManager.GenerateJwtToken(email, false);
            return Ok(new {sessionToken});
        }
    }


}
