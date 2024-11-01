using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarSearchAPI.Models;
using CarSearchAPI.Services;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;

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
    }
}
