using CarSearchAPI.DTOs.Users;
using CarSearchAPI.Models;
using CarSearchAPI.Repositories.Abstractions;
using CarSearchAPI.Services.UserServices;
using Moq;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSearchAPI.Tests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService( _userRepositoryMock.Object);
        }

        [Fact]
        public async Task UserService_ServeNewCreatedUser_WhenUserDoesNotExist_ShouldReturnTrue()
        {
            // Arrange
            var email = "example@ex.com";
            var userInfo = new NewUserInfoDto
            {
                name = "John",
                surname = "Doe",
                birthDate = new DateOnly(1990, 1, 1),
                licenceDate = new DateOnly(2020, 1, 1)
            };

            _userRepositoryMock
                .Setup(repo => repo.GetApplicationUserOrNullByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);

            _userRepositoryMock
                .Setup(repo => repo.AddApplicationUserAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.ServeNewCreatedUser(email, userInfo);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserService_ServeNewCreatedUser_WhenUserDoesExist_ShouldReturnFalse()
        {
            // Arrange
            var email = "email@ex.com";
            var userInfo = new NewUserInfoDto
            {                
                name = "John",
                surname = "Doe",
                birthDate = new DateOnly(1990, 1, 1),
                licenceDate = new DateOnly(2020, 1, 1)
            };
            var existingUser = new ApplicationUser
            {
                Email = email,
                Name = "Existing",
                Surname = "User",
                BirthDate = new DateOnly(1980, 1, 1),
                LicenceDate = new DateOnly(2010, 1, 1)
            };

            _userRepositoryMock
                .Setup(repo => repo.GetApplicationUserOrNullByEmailAsync(email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.ServeNewCreatedUser(email, userInfo);

            // Assert
            Assert.False(result);
        }
    }
}
