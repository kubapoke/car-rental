using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using CarSearchAPI.Abstractions;

namespace CarSearchAPI.Services
{
    public class GoogleAuthService : IAuthService
    {        
        public async Task<bool> VerifyToken(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload != null;

            }
            catch (InvalidJwtException)
            {
                return false;
            }

        }
    }
}
