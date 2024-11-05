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

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken); // information extracted from token
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.email == payload.Email); // check is user exist in the database

            if (user != null)
            {
                var jwtToken = sessionTokenManager.GenerateJwtToken(user.email, false); // create normal token for old user
                return Ok(new { jwtToken, isNewUser = false });
            }
            else
            {
                var jwtToken = sessionTokenManager.GenerateJwtToken(payload.Email, true); // create temporary token to finish registration
                return Ok(new { jwtToken, isNewUser = true });
            }
        }

        [Authorize] // only users with appropriate bearer token can use this method
        [HttpPost("complete-registration")]
        // create new user, based on the information from NewUserForm from front-end, information will be transformed from json to NewUserDto
        public async Task<IActionResult> CompleteRegistration([FromBody] NewUserInfoDto userInfo)
        {
            // User is .net feature, it contains information from bearer token
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; // we get an email from bearer token
            
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid temporary session");
            }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            
            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.email == email); // double check is user in database
            if (user == null)
            {
                user = new ApplicationUser
                {
                    email = email,
                    name = userInfo.name,
                    surname = userInfo.surname,
                    birthDate = userInfo.birthDate,
                    licenceDate = userInfo.licenceDate
                };

                // adding user to the database
                _context.applicationUsers.Add(user); 
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("This user already exist");
            }

            // creation of new token
            SessionTokenManager sessionTokenManager = new SessionTokenManager();
            var sessionToken = sessionTokenManager.GenerateJwtToken(email, false);
            return Ok(new {sessionToken});
        }
    }


}
