using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;

namespace CarSearchAPI.Abstractions
{
    public interface IConfirmationTokenService
    {
        public string GenerateConfirmationToken(OfferDto info);

        public ClaimsPrincipal ValidateConfirmationToken(string token);

        public bool ValidateOfferClaim(ClaimsPrincipal claimsPrincipal);
    }
}
