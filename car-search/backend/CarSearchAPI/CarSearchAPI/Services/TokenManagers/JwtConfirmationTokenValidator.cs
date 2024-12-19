using CarSearchAPI.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarSearchAPI.Services.TokenManagers
{
    public class JwtConfirmationTokenValidator : IConfirmationTokenValidator
    {

        public ClaimsPrincipal ValidateConfirmationToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_CONFIRMATION_TOKEN_SECRET_KEY"));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        public bool ValidateOfferClaim(ClaimsPrincipal claimsPrincipal)
        {
            string offerId = claimsPrincipal.FindFirst("OfferId")?.Value;
            string email = claimsPrincipal.FindFirst("Email")?.Value;
            string companyName = claimsPrincipal.FindFirst("CompanyName")?.Value;

            if (string.IsNullOrEmpty(offerId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(companyName))
            {
                return false;
            }

            return true;
        }
    }
}
