using CarRentalAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalAPI.Services.PasswordServices;

namespace CarRentalAPI.Tests.ServicesTests
{
    public class Hmacsha256PasswordServiceTests
    {
        private readonly Hmacsha256PasswordService _passwordService;

        public Hmacsha256PasswordServiceTests()
        {
            _passwordService = new Hmacsha256PasswordService();
        }

        [Fact]
        public void PasswordHasher_HashPassword_ShouldReturnNotEmptyHashAndSalt()
        {
            // Arrange
            string password = "Example123";

            // Act
            (string hash, string salt) = _passwordService.HashPassword(password);

            // Assert
            Assert.False(hash.IsNullOrEmpty(), "Hash should not be null or empty");
            Assert.False(salt.IsNullOrEmpty(), "Salt should not be null or empty");
        }

        [Fact]
        public void PasswordHasher_HashPassword_ShouldReturnDifferentSaltsAndHashesForTheSamePassword()
        {
            // Arrange
            string password = "Example123";

            // Act
            (string hash1, string salt1) = _passwordService.HashPassword(password);
            (string hash2, string salt2) = _passwordService.HashPassword(password);

            // Assert
            Assert.NotEqual(salt1, salt2);
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void PasswordHasher_VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            string password = "Example123";
            (string hash, string salt) = _passwordService.HashPassword(password);

            // Act
            bool result = _passwordService.VerifyPassword(password, hash, salt);

            // Assert
            Assert.True(result, "Should be true for correct password");
        }

        [Fact]
        public void PasswordHasher_VerifyPassword_ShouldReturnFalseForWrongPassword()
        {
            // Arrange
            string rightPassword = "Example123";
            (string hash, string salt) = _passwordService.HashPassword(rightPassword);
            string wrongPassword = "WrongPassword";

            // Act
            bool result = _passwordService.VerifyPassword(wrongPassword, hash, salt);

            // Assert
            Assert.False(result, "Should be false for wrong password");
        }

        [Theory]
        [InlineData(null, "hash", "salt")]
        [InlineData("password", null, "salt")]
        [InlineData("password", "hash", null)]
        [InlineData("", "hash", "salt")]
        [InlineData("password", "", "salt")]
        [InlineData("password", "hash", "")]
        public void PasswordHasher_VerifyPassword_ShouldReturnFalseForWrongInputs(string password, string hash, string salt)
        {
            // Arrange

            // Act
            bool result = _passwordService.VerifyPassword(password, hash, salt);

            // Assert
            Assert.False(result, "Should return false for wrong input");
        }


    }
}
