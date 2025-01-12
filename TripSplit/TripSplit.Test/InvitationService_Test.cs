using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripSplit.Application;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;
using TripSplit.DataAccess;
using System.Linq.Expressions;

namespace TripSplit.Test
{
    [TestClass]
    public class InvitationService_Test
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private Mock<DbSet<Invitation>> _mockInvitations;
        private Mock<DbSet<Trip>> _mockTrips;
        private Mock<DbSet<User>> _mockUsers;
        private Mock<DbSet<TripUser>> _mockTripUsers;
        private InvitationService _invitationService;

        [TestInitialize]
        public void SetUp()
        {
            // Mock the DbSets
            _mockDbContext = new Mock<ApplicationDbContext>();

            _mockInvitations = new Mock<DbSet<Invitation>>();
            _mockTrips = new Mock<DbSet<Trip>>();
            _mockUsers = new Mock<DbSet<User>>();
            _mockTripUsers = new Mock<DbSet<TripUser>>();

            // Setup DbContext to return mocked DbSets
            _mockDbContext.Setup(db => db.Invitations).Returns(_mockInvitations.Object);
            _mockDbContext.Setup(db => db.Trips).Returns(_mockTrips.Object);
            _mockDbContext.Setup(db => db.Users).Returns(_mockUsers.Object);
            _mockDbContext.Setup(db => db.TripUsers).Returns(_mockTripUsers.Object);

            // Initialize the service with the mocked context
            _invitationService = new InvitationService(_mockDbContext.Object);
        }

        [TestMethod]
        public async Task SendInvitation_ShouldAddInvitation()
        {
            // Arrange
            var tripId = 1;
            var userId = "user1";
            var trip = new Trip { Id = tripId, Name = "Trip to Paris", Destination = "Paris" };
            var user = new User { Id = userId, Email = "user1@example.com" };

            _mockTrips.Setup(t => t.FindAsync(tripId)).ReturnsAsync(trip);
            _mockUsers.Setup(u => u.FindAsync(userId)).ReturnsAsync(user);
            _mockInvitations.Setup(i => i.FindAsync(tripId, userId)).ReturnsAsync((Invitation)null); // No existing invitation

            // Act
            await _invitationService.SendInvitation(tripId, userId);

            // Assert
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockInvitations.Verify(i => i.Add(It.IsAny<Invitation>()), Times.Once);
        }

        [TestMethod]
        public async Task AcceptInvitation_ShouldAddTripUser()
        {
            // Arrange
            var tripId = 1;
            var userId = "user1";
            var invitation = new Invitation { TripId = tripId, UserId = userId };
            var tripUser = new TripUser { TripId = tripId, UserId = userId };

            _mockInvitations.Setup(i => i.FindAsync(tripId, userId)).ReturnsAsync(invitation);
            _mockTripUsers.Setup(tu => tu.Add(It.IsAny<TripUser>()));

            // Act
            await _invitationService.AcceptInvitation(tripId, userId);

            // Assert
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockInvitations.Verify(i => i.Remove(invitation), Times.Once);
            _mockTripUsers.Verify(tu => tu.Add(It.IsAny<TripUser>()), Times.Once);
        }

        [TestMethod]
        public async Task RejectInvitation_ShouldRemoveInvitation()
        {
            // Arrange
            var tripId = 1;
            var userId = "user1";
            var invitation = new Invitation { TripId = tripId, UserId = userId };

            _mockInvitations.Setup(i => i.FindAsync(tripId, userId)).ReturnsAsync(invitation);

            // Act
            await _invitationService.RejectInvitation(tripId, userId);

            // Assert
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockInvitations.Verify(i => i.Remove(invitation), Times.Once);
        }

        [TestMethod]
        public async Task GetInvitationsForUser_ShouldReturnListOfInvitations()
        {
            // Arrange
            var userId = "user1";
            var invitations = new List<Invitation>
            {
                new Invitation { TripId = 1, UserId = userId, IsDenied = false, Trip = new Trip { Name = "Trip to Paris", Destination = "Paris" } },
                new Invitation { TripId = 2, UserId = userId, IsDenied = false, Trip = new Trip { Name = "Trip to London", Destination = "London" } }
            };

            _mockInvitations.Setup(i => i.Where(It.IsAny<Func<Invitation, bool>>()))
                .Returns(invitations.Where(inv => inv.UserId == userId && !inv.IsDenied).AsQueryable());

            // Act
            var result = await _invitationService.GetInvitationsForUser(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Trip to Paris", result[0].TripName);
            Assert.AreEqual("Trip to London", result[1].TripName);
        }

        [TestMethod]
        public async Task InviteUserByEmail_ShouldThrowExceptionIfUserAlreadyExistsInTrip()
        {
            // Arrange
            var tripId = 1;
            var email = "user1@example.com";
            var user = new User { Id = "user1", Email = email };

            _mockUsers.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Simulate that the user is already in the trip using an Expression
            _mockDbContext.Setup(db => db.TripUsers
                .AnyAsync(It.IsAny<Expression<Func<TripUser, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _invitationService.InviteUserByEmail(tripId, email));
        }


    }
}
