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
        // this create bearer tokens
        public string GenerateJwtToken(string email, bool isTemporary)
        {
            var tokenHandelr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET_KEY"));
            List<Claim> claims = new List<Claim>();
            Claim emailClaim = new Claim(JwtRegisteredClaimNames.Email, email);
            claims.Add(emailClaim); // claims represent information, which will be inside token
            double expirationMinutes; // that is time, in which token will be valid
            if (isTemporary)
            {
                Claim protoUserClaim = new Claim("ProtoUser", "");
                claims.Add(protoUserClaim);
                expirationMinutes = 10;
            }
            else
            {
                Claim legitUserClaim = new Claim("LegitUser", "");
                claims.Add(legitUserClaim);
                expirationMinutes = 60;
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // add claims to the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), // add key
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes) // add time
            };
            var token = tokenHandelr.CreateToken(tokenDescriptor);


            return tokenHandelr.WriteToken(token);
        }

    }
}
