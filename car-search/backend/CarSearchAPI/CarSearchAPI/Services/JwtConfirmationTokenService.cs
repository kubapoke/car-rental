using CarSearchAPI.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CarSearchAPI.Services
{
    public class JwtConfirmationTokenService : IConfirmationTokenService
    {
        public string GenerateConfirmationToken(OfferDto info)
        {
            var tokenHandelr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_CONFIRMATION_TOKEN_SECRET_KEY"));

            var claims = new[]
            {
                new Claim("Email", info.Email),
                new Claim("Price", info.Price.ToString()),
                new Claim("CompanyName", info.CompanyName),
                new Claim("StartDate", info.StartDate.ToString()),
                new Claim("EndDate", info.EndDate.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), 
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
            var token = tokenHandelr.CreateToken(tokenDescriptor);

            return tokenHandelr.WriteToken(token);
        }

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

        public bool ValidateAllClaims(ClaimsPrincipal claimsPrincipal)
        {
            string email = claimsPrincipal.FindFirst("Email")?.Value;
            string price = claimsPrincipal.FindFirst("Price")?.Value;
            string companyName = claimsPrincipal.FindFirst("CompanyName")?.Value;
            string startDate = claimsPrincipal.FindFirst("StartDate")?.Value;
            string endDate = claimsPrincipal.FindFirst("EndDate")?.Value;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(companyName) ||
                string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return false;
            }
            return true;
            
        }
    }
}
