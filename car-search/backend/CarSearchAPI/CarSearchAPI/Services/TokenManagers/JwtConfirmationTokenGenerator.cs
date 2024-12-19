using CarSearchAPI.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarSearchAPI.DTOs.CarRental;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CarSearchAPI.Services.TokenManagers
{
    public class JwtConfirmationTokenGenerator : IConfirmationTokenGenerator
    {
        public string GenerateConfirmationToken(OfferDto info)
        {
            var tokenHandelr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_CONFIRMATION_TOKEN_SECRET_KEY"));

            var claims = new[]
            {
                new Claim("OfferId", info.OfferId),
                new Claim("Email", info.Email),
                new Claim("CompanyName", info.CompanyName),
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
    }
}
