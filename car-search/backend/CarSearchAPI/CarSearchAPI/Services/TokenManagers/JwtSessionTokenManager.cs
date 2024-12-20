using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using CarSearchAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services.TokenManagers
{
    public class JwtSessionTokenManager : ISessionTokenManager
    {

        public string GetSessionToken(string email, bool isTemporary)
        {
            string stringKey = GetSekretKeyAsString();
            string token = GenerateToken(email, isTemporary, stringKey);
            return token;
        }

        public string GenerateToken(string email, bool isTemporary, string stringKey)
        {
            var key = Encoding.UTF8.GetBytes(stringKey);
            List<Claim> claims = new List<Claim>();
            Claim emailClaim = new Claim(JwtRegisteredClaimNames.Email, email);
            claims.Add(emailClaim);

            (Claim userClaim, double expirationMinutes) = GetClaimAndExpirationTime(isTemporary);
            claims.Add(userClaim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }

        public (Claim, double) GetClaimAndExpirationTime(bool isTemporary)
        {
            double expirationMinutes;
            if (isTemporary)
            {
                Claim protoUserClaim = new Claim("ProtoUserClaim", "1");
                expirationMinutes = 10;
                return (protoUserClaim, expirationMinutes);
            }
            else
            {
                Claim legitUserClaim = new Claim("LegitUserClaim", "1");
                expirationMinutes = 60;
                return (legitUserClaim, expirationMinutes);
            }
        }

        private string GetSekretKeyAsString()
        {
            return Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY");
        }

    }
}
