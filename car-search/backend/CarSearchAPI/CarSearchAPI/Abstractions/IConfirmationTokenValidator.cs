using System.Security.Claims;

namespace CarSearchAPI.Abstractions
{
    public interface IConfirmationTokenValidator
    {
        public ClaimsPrincipal ValidateConfirmationToken(string token);
        public bool ValidateOfferClaim(ClaimsPrincipal claimsPrincipal);
    }
}
