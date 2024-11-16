using CarSearchAPI.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CarSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly CarSearchDbContext _context;
        private readonly IConfirmationTokenService _confirmationTokenService;

        public RentsController(CarSearchDbContext context, IConfirmationTokenService confirmationTokenService)
        {
            _context = context;
            _confirmationTokenService = confirmationTokenService;
        }

        [HttpGet("new-rent-confirm")]
        public async Task<IActionResult> NewRentConfirm(string token)
        {
            try
            {
                var claimsPrincipal = _confirmationTokenService.ValidateConfirmationToken(token);
                
                if (!_confirmationTokenService.ValidateAllClaims(claimsPrincipal)) { return BadRequest("Invalid token"); }
                
                

                return Ok();
            }
            catch (SecurityTokenExpiredException)
            {
                return BadRequest("Toke has expired");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");
            }
           
        }
    }
}
