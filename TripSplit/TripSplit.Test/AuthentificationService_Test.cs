using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using TripSplit.Application;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Exceptions;
using TripSplit.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;

namespace TripSplit.Test
{
    [TestClass]
    public class AuthenticationService_Test
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IEmailService> _mockEmailService;
        private Mock<ITokenService> _mockTokenService;
        private AuthenticationService _authService;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _mockEmailService = new Mock<IEmailService>();
            _mockTokenService = new Mock<ITokenService>();

            _authService = new AuthenticationService(_mockUserManager.Object, _mockEmailService.Object, _mockTokenService.Object);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ThrowsInvalidUserCredentialsException()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidUserCredentialsException>(async () =>
                await _authService.Login(loginDto));
        }

        [TestMethod]
        public async Task Login_InvalidPassword_ThrowsInvalidUserCredentialsException()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var user = new User { Email = "test@example.com" };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidUserCredentialsException>(async () =>
                await _authService.Login(loginDto));
        }

        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsLoginResult()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var user = new User { Email = "test@example.com" };
            var token = "fake-jwt-token";
            var userDto = new UserDto { FirstName = "John", LastName = "Doe", Email = "test@example.com" };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(true);
            _mockTokenService.Setup(ts => ts.CreateTokenAsync(user)).ReturnsAsync(token);
            MappingProfile.UserToUserDto(user); // Mapping logic should work accordingly.

            // Act
            var result = await _authService.Login(loginDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(token, result.Token);
            Assert.AreEqual(userDto.Email, result.User.Email);
        }

        [TestMethod]
        public async Task Register_EmailAlreadyTaken_ThrowsEmailTakenException()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                RegisterDto = new RegisterDto { Email = "test@example.com", Password = "password", FirstName = "John", LastName = "Doe" },
                HttpRequest = new Mock<HttpRequest>().Object

            };

            var existingUser = new User { Email = "test@example.com" };

            _mockUserManager.Setup(um => um.FindByEmailAsync(registerRequest.RegisterDto.Email)).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<EmailTakenException>(async () =>
                await _authService.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_SuccessfullyRegistersUser()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                RegisterDto = new RegisterDto { Email = "test@example.com", Password = "password", FirstName = "John", LastName = "Doe" },
                HttpRequest = new Mock<HttpRequest>().Object

            };

            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(registerRequest.RegisterDto.Email)).ReturnsAsync(It.IsAny<User>());
            //_mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), registerRequest.RegisterDto.Password)).ReturnsAsync(IdentityResult.Success);
            //_mockUserManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("confirmation-token");

            // Act
            await _authService.Register(registerRequest);

            // Assert
            _mockUserManager.Verify(um => um.CreateAsync(It.IsAny<User>(), registerRequest.RegisterDto.Password), Times.Once);
            //_mockEmailService.Verify(es => es.SendConfirmationEmail(It.IsAny<User>(), "confirmation-token", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmEmail_UserNotFound_ThrowsInvalidUserCredentialsException()
        {
            // Arrange
            var request = new EmailConfirmationRequest { UserId = "nonexistent-user-id", ConfirmationToken = "token" };

            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidUserCredentialsException>(async () =>
                await _authService.ConfirmEmail(request));
        }

        [TestMethod]
        public async Task ConfirmEmail_SuccessfulConfirmation_ReturnsUserId()
        {
            // Arrange
            var user = new User { Id = "user-id", Email = "test@example.com" };
            var request = new EmailConfirmationRequest { UserId = "user-id", ConfirmationToken = "confirmation-token" };

            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ConfirmEmailAsync(user, request.ConfirmationToken)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.ConfirmEmail(request);

            // Assert
            Assert.AreEqual(user.Id, result);
        }
    }
}
