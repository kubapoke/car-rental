using CarSearchAPI.Abstractions;
using CarSearchAPI.Models;
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
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;

        public RentsController(CarSearchDbContext context, IConfirmationTokenService confirmationTokenService, IEnumerable<IExternalDataProvider> dataProviders)
        {
            _context = context;
            _confirmationTokenService = confirmationTokenService;
            _dataProviders = dataProviders;
        }

        [HttpGet("new-rent-confirm")]
        public async Task<IActionResult> NewRentConfirm(string token)
        {
            try
            {
                var claimsPrincipal = _confirmationTokenService.ValidateConfirmationToken(token);
                
                if (!_confirmationTokenService.ValidateAllClaims(claimsPrincipal)) { return BadRequest("Invalid token"); }

                IExternalDataProvider? activeProvider = null;

                foreach (var provider in _dataProviders)
                {
                    if (provider.GetProviderName() == claimsPrincipal.FindFirst("CompanyName")?.Value) 
                    {
                        activeProvider = provider;
                        break;
                    }
                }
                
                if (activeProvider == null) { return BadRequest("Invalid provider name"); }
                
                var results = await activeProvider.CreateNewRentAsync(claimsPrincipal);

                Rent newRent = new Rent()
                {
                    UserEmail = results.Email,
                    Brand = results.Brand,
                    Model = results.Model,
                    StartDate = results.StartDate,
                    EndDate = results.EndDate
                };

                _context.rents.Add(newRent);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (SecurityTokenExpiredException)
            {
                return BadRequest("Token has expired");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");
            }
           
        }
    }
}
