using CarSearchAPI.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;
using System.Text;

namespace CarSearchAPI.Services
{
    public class JwtConfirmationTokenService : IConfirmationTokenService
    {
        public string GenerateConfirmationToken(OfferDto info)
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
