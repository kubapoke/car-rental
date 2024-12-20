using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.Rents;
using CarSearchAPI.Enums;
using CarSearchAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CarSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly CarSearchDbContext _context;
        private readonly IRentService _rentService;
        private readonly IConfirmationTokenValidator _confirmationTokenValidator;
        private readonly IEnumerable<IExternalDataProvider> _dataProviders;

        public RentsController(CarSearchDbContext context, IConfirmationTokenValidator confirmationTokenValidator, IEnumerable<IExternalDataProvider> dataProviders, IRentService rentService)
        {
            _context = context;
            _confirmationTokenValidator = confirmationTokenValidator;
            _dataProviders = dataProviders;
            _rentService = rentService;
        }

        [HttpGet("new-rent-confirm")]
        public async Task<IActionResult> NewRentConfirm(string token)
        {
            try
            {
                var claimsPrincipal = _confirmationTokenValidator.ValidateConfirmationToken(token);                
                if (!_confirmationTokenValidator.ValidateOfferClaim(claimsPrincipal)) { return BadRequest("Invalid token"); }

                IExternalDataProvider? activeProvider = GetActiveDataProvider(claimsPrincipal.FindFirst("CompanyName")?.Value);                
                if (activeProvider == null) { return BadRequest("Invalid provider name"); }
                
                var results = await activeProvider.CreateNewRentAsync(claimsPrincipal);                

                await _rentService.CreateNewRentAsync(results, activeProvider.GetProviderName());

                return Ok();
            }
            catch (SecurityTokenExpiredException)
            {
                return BadRequest("Token has expired");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token, this message may be showing, because you already confirmed your rent.");
            }
           
        }

        [Authorize]
        [HttpGet("get-user-rents")]
        public async Task<IActionResult> GetUserRents()
        {
            var email = User.Claims.FirstOrDefault(c => ClaimTypes.Email == c.Type)?.Value;
            if (email == null)
            {
                return Unauthorized("You are not logged in");
            }

            var rentInfoList = await _rentService.GetRentInfoListByEmailAsync(email);

            return Ok(rentInfoList);
        }

        [Authorize]
        [HttpPost("return-car")]
        public async Task<IActionResult> ReturnCar([FromBody]int rentId)
        {
            var rent = await _rentService.GetRenOrNullByIdAsync(rentId);
            if (rent == null)
            {
                return NotFound("Rent not found");
            }

            IExternalDataProvider? activeProvider = GetActiveDataProvider(rent.RentalCompanyName);                
            if (activeProvider == null) { return BadRequest("Invalid provider name"); }

            var isSuccess = await activeProvider.SetRentStatusReadyToReturnAsync(rent.RentalCompanyRentId);
            if (!isSuccess)
             {
                 return BadRequest();
             }

            await _rentService.SetRentStatusReturnedAsync(rent);
            
            return Ok();
        }
        
        private IExternalDataProvider? GetActiveDataProvider(string givenProviderName)
        {
            IExternalDataProvider? activeProvider = null;

            foreach (var provider in _dataProviders)
            {
                if (provider.GetProviderName() == givenProviderName)
                {
                    activeProvider = provider;
                    break;
                }
            }

            return activeProvider;
        }
    }
}
