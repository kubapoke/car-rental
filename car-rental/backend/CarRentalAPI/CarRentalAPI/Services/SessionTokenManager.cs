using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Services
{
    public class SessionTokenManager
    {
        public string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("CAR_RENTAL_SECRET_KEY"));
            List<Claim> claims = new List<Claim>();
            Claim userNameClaim = new Claim("UserName", userName);
            claims.Add(userNameClaim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // add claims to the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), // add key
                Expires = DateTime.UtcNow.AddMinutes(60) // add time
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
    }
}
