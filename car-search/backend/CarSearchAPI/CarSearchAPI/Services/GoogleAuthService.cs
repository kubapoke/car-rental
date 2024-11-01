using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CarSearchAPI.Services
{
    public class GoogleAuthService
    {
        public async Task<bool> VerifyGoogleToken(string idToken)
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
