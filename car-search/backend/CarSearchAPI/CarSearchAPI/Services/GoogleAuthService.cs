using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CarSearchAPI.Services
{
    public class GoogleAuthService
    {
        // this will check if google token is valid
        public async Task<bool> VerifyGoogleToken(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken); // payload is information extracted from google token
                return payload != null;

            }
            catch (InvalidJwtException)
            {
                return false;
            }

        }
    }
}
