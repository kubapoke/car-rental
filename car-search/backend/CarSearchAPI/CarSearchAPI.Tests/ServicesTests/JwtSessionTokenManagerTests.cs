using CarSearchAPI.Services.TokenManagers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarSearchAPI.Tests.ServicesTests
{
    public class JwtSessionTokenManagerTests
    {
        private const string SampleKey = "ThisIsASecretKeyForTestingPurposes";
        private const string SampleEmail = "test@example.com";

        [Fact]
        public void JwtSessionTokenManager_GetClaimAndExpirationTime_ProtoUser_ShouldReturnProtoClaimAnd10minuts()
        {
            // Arrange
            bool isTemporary = true;
            JwtSessionTokenManager tokenManager = new JwtSessionTokenManager();

            // Act
            (Claim userClaim, double expirationMin) = tokenManager.GetClaimAndExpirationTime(isTemporary);

            // Assert
            Assert.NotNull(userClaim);
            Assert.Equal("ProtoUserClaim", userClaim.Type);
            Assert.Equal(10, expirationMin);
        }

        [Fact]
        public void JwtSessionTokenManager_GetClaimAndExpirationTime_LegitUser_ShouldReturnLegitClaimAnd60minuts()
        {
            // Arrange
            bool isTemporary = false;
            JwtSessionTokenManager tokenManager = new JwtSessionTokenManager();

            // Act
            (Claim userClaim, double expirationMin) = tokenManager.GetClaimAndExpirationTime(isTemporary);

            // Assert
            Assert.NotNull(userClaim);
            Assert.Equal("LegitUserClaim", userClaim.Type);
            Assert.Equal(60, expirationMin);
        }

        [Fact]
        public void JwtSessionTokenManager_GenereateToken_LegitToken_ShouldReturnTokenForLegitUser()
        {
            // Arrange 
            bool isTemporary = false;
            JwtSessionTokenManager tokenManager = new JwtSessionTokenManager();

            // Act
            string token = tokenManager.GenerateToken(SampleEmail, isTemporary, SampleKey);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SampleKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            Assert.NotNull(principal);
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Email && c.Value == SampleEmail);
            Assert.Contains(principal.Claims, c => c.Type == "LegitUserClaim");
        }

        [Fact]
        public void JwtSessionTokenManager_GenerateToken_ProtoToken_ShouldReturnTokenForProtoUser()
        {
            // Arrange 
            bool isTemporary = true;
            JwtSessionTokenManager tokenManager = new JwtSessionTokenManager();

            // Act
            string token = tokenManager.GenerateToken(SampleEmail, isTemporary, SampleKey);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SampleKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            Assert.NotNull(principal);
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Email && c.Value == SampleEmail);
            Assert.Contains(principal.Claims, c => c.Type == "ProtoUserClaim");
        }
    }
}
