using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application;
using TripSplit.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripSplit.Test
{
    [TestClass]
    public class TokenService_Test
    {
        private Mock<IConfiguration> _mockConfiguration;
        private TokenService _tokenService;

        [TestInitialize]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            // Mock JWTConfig section
            var jwtConfigSection = new Mock<IConfigurationSection>();
            jwtConfigSection.Setup(s => s["secret"]).Returns("MySuperSecretKey");
            jwtConfigSection.Setup(s => s["validIssuer"]).Returns("TestIssuer");
            jwtConfigSection.Setup(s => s["validAudience"]).Returns("TestAudience");
            jwtConfigSection.Setup(s => s["expiresIn"]).Returns("30");

            _mockConfiguration.Setup(c => c.GetSection("JWTConfig")).Returns(jwtConfigSection.Object);

            _tokenService = new TokenService(_mockConfiguration.Object);
        }

        [TestMethod]
        public async Task CreateTokenAsync_GeneratesTokenSuccessfully()
        {
            // Arrange
            var user = new User { Email = "user@example.com" };

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Contains("user@example.com")); // Checks if the user's email is included in the token claims.
        }

        [TestMethod]
        public async Task CreateTokenAsync_GeneratesTokenWithCorrectClaims()
        {
            // Arrange
            var user = new User { Email = "user@example.com" };

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            Assert.IsNotNull(emailClaim);
            Assert.AreEqual("user@example.com", emailClaim.Value);
        }

        [TestMethod]
        public async Task CreateTokenAsync_ThrowsException_WhenNoConfigurationProvided()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetSection("JWTConfig")).Returns((IConfigurationSection)null);

            var user = new User { Email = "user@example.com" };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
                await _tokenService.CreateTokenAsync(user));
        }
    }
}
