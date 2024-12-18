using CarSearchAPI.Services;
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


    }
}
