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
        private readonly IUserService _userService;

        private readonly IAuthService _authService;

        private readonly ISessionTokenManager _sessionTokenManager;

        public AuthController(IAuthService authService, ISessionTokenManager sessionTokenManager, IUserService userService)
        {
            _authService = authService;
            _sessionTokenManager = sessionTokenManager;
            _userService = userService;
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] string idToken)
        {            
            bool isValid = await _authService.VerifyTokenAsync(idToken);

            if (!isValid)
            {
                return Unauthorized();
            }

            (string jwtToken, bool isNewUser) = await _authService.GetTokenAndFlagAsync(idToken);

            return Ok(new { jwtToken, isNewUser });
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

            
            if (!(await _userService.ServeNewCreatedUser(email, userInfo))) 
            { 
                return BadRequest("User already exists"); 
            }

            var sessionToken = _sessionTokenManager.GetSessionToken(email, false);
            return Ok(new {sessionToken});
        }
    }


}
