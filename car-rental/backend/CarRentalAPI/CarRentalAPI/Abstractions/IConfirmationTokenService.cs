using CarRentalAPI.DTOs.Offers;
using System.Security.Claims;

namespace CarRentalAPI.Abstractions
{
    public interface IConfirmationTokenService
    {
        public string GenerateConfirmationToken(OfferInfoForNewRentDto info);

        public ClaimsPrincipal ValidateConfirmationToken(string token);
    }
}
