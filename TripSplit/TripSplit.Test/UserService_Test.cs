using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripSplit.Application;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripSplit.Test
{
    [TestClass]
    public class UserService_Test
    {
        private Mock<UserManager<User>> _mockUserManager;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userService = new UserService(_mockUserManager.Object);
        }

        [TestMethod]
        public async Task GetUserById_ReturnsUserDto()
        {
            // Arrange
            var userId = "user1";
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserManager.Setup(manager => manager.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User not found")]
        public async Task GetUserById_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var userId = "user1";
            _mockUserManager.Setup(manager => manager.FindByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            await _userService.GetUserById(userId);
        }

        [TestMethod]
        public async Task UpdateUser_UpdatesUserSuccessfully()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = "user1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var user = new User
            {
                Id = userDto.Id,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Email = "old.email@example.com"
            };

            _mockUserManager.Setup(manager => manager.FindByIdAsync(userDto.Id)).ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.UpdateUser(userDto);

            // Assert
            Assert.AreEqual(userDto.FirstName, user.FirstName);
            Assert.AreEqual(userDto.LastName, user.LastName);
            Assert.AreEqual(userDto.Email, user.Email);
            _mockUserManager.Verify(manager => manager.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User update failed")]
        public async Task UpdateUser_ThrowsException_WhenUpdateFails()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = "user1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var user = new User
            {
                Id = userDto.Id,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Email = "old.email@example.com"
            };

            _mockUserManager.Setup(manager => manager.FindByIdAsync(userDto.Id)).ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            await _userService.UpdateUser(userDto);
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "user1", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new User { Id = "user2", FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            };

            _mockUserManager.Setup(manager => manager.Users).Returns(users.AsQueryable());

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("John", result.First().FirstName);
            Assert.AreEqual("Jane", result.Last().FirstName);
        }

        [TestMethod]
        public async Task DeleteUser_DeletesUserSuccessfully()
        {
            // Arrange
            var userId = "user1";
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserManager.Setup(manager => manager.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            _mockUserManager.Verify(manager => manager.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User not found")]
        public async Task DeleteUser_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var userId = "user1";
            _mockUserManager.Setup(manager => manager.FindByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            await _userService.DeleteUser(userId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User deletion failed")]
        public async Task DeleteUser_ThrowsException_WhenDeletionFails()
        {
            // Arrange
            var userId = "user1";
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserManager.Setup(manager => manager.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(manager => manager.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            await _userService.DeleteUser(userId);
        }
    }
}
