using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Offers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Services
{
    public class JwtConfirmationTokenService : IConfirmationTokenService
    {
        public string GenerateConfirmationToken(OfferInfoForNewRentDto info)
        {
            var tokenHandelr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_CONFIRMATION_TOKEN_SECRET_KEY"));
            return "";
        }

        public ClaimsPrincipal ValidateConfirmationToken(string token)
        {
            return new ClaimsPrincipal();
        }
    }
}
