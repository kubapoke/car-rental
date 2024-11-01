using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using CarSearchAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarSearchAPI.Services
{
    public class SessionTokenManager
    {
        public string GenerateJwtToken(string email, bool isTemporary)
        {
            var tokenHandelr = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY"));
            var claim = new Claim(JwtRegisteredClaimNames.Email, email);
            double expirationMinutes;
            if (isTemporary)
            {
                expirationMinutes = 10;
            }
            else
            {
                expirationMinutes = 60;
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { claim }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };
            var token = tokenHandelr.CreateToken(tokenDescriptor);


            return tokenHandelr.WriteToken(token);
        }

    }
}
