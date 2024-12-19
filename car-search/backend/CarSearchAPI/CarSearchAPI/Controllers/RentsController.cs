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

            var rentInfoList = _rentService.GetRentInfoListByEmailAsync(email);

            return Ok(rentInfoList);
        }

        [Authorize]
        [HttpPost("return-car")]
        public async Task<IActionResult> ReturnCar([FromBody]int rentId)
        {
            var rent = await _context.rents.FirstOrDefaultAsync(r => r.RentId == rentId);
            if (rent == null)
            {
                return NotFound("Rent not found");
            }

            IExternalDataProvider? activeProvider = null;

            foreach (var provider in _dataProviders)
            {
                if (provider.GetProviderName() == rent.RentalCompanyName) 
                {
                    activeProvider = provider;
                    break;
                }
            }
                
            if (activeProvider == null) { return BadRequest("Invalid provider name"); }

            var rentalApiResponse = await activeProvider.SetRentStatusReadyToReturnAsync(rent.RentalCompanyRentId);
            if (!rentalApiResponse)
             {
                 return BadRequest();
             }

            rent.Status = RentStatuses.Returned;
            // TODO: check if we need these two lines stuff below
            _context.rents.Update(rent);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
