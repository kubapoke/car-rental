using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using CarSearchAPI.Abstractions;
using CarSearchAPI.Repositories.Abstractions;

namespace CarSearchAPI.Services
{
    public class GoogleAuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private ISessionTokenManager _sessionTokenManager;

        public GoogleAuthService(IUserRepository userRepository, ISessionTokenManager sessionTokenManager)
        {
            _userRepository = userRepository;
            _sessionTokenManager = sessionTokenManager;
        }

        public async Task<(string token, bool isTemporary)> GetTokenAndFlagAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = await _userRepository.GetApplicationUSerByEmailAsync(payload.Email);

            if (user != null)
            {
                var jwtToken = _sessionTokenManager.GetSessionToken(user.Email, false);
                return (jwtToken, false);
            }
            else
            {
                var jwtToken = _sessionTokenManager.GetSessionToken(payload.Email, true);
                return (jwtToken, true);
            }
        }

        public async Task<bool> VerifyTokenAsync(string idToken)
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
