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
    public class TripService_Test
    {
        private Mock<ITripRepository> _mockTripRepository;
        private Mock<ITripUserRepository> _mockTripUserRepository;
        private TripService _tripService;

        [TestInitialize]
        public void Setup()
        {
            _mockTripRepository = new Mock<ITripRepository>();
            _mockTripUserRepository = new Mock<ITripUserRepository>();
            _tripService = new TripService(_mockTripRepository.Object, _mockTripUserRepository.Object);
        }

        [TestMethod]
        public async Task GetUserTrips_ReturnsUserTrips()
        {
            // Arrange
            var userId = "user1";
            var trips = new List<Trip>
            {
                new Trip { Id = 1, Name = "Trip 1", Destination = "Paris" },
                new Trip { Id = 2, Name = "Trip 2", Destination = "London" }
            };

            _mockTripRepository.Setup(repo => repo.GetTripsByUserId(userId)).ReturnsAsync(trips);

            // Act
            var result = await _tripService.GetUserTrips(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Trip 1", result.First().Name);
        }

        [TestMethod]
        public async Task CreateTrip_AddsTripAndReturnsId()
        {
            // Arrange
            var createTripDto = new CreateTripDto
            {
                Name = "Trip to Paris",
                Destination = "Paris",
                Description = "A fun trip to Paris",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5)
            };
            var newTripId = 123;

            _mockTripRepository
                .Setup(repo => repo.AddTrip(It.IsAny<Trip>()))
                .ReturnsAsync(newTripId);

            // Act
            var result = await _tripService.CreateTrip(createTripDto);

            // Assert
            Assert.AreEqual(newTripId, result);
            _mockTripRepository.Verify(repo => repo.AddTrip(It.IsAny<Trip>()), Times.Once);
        }

        [TestMethod]
        public async Task AddUserToTrip_AddsUserSuccessfully()
        {
            // Arrange
            var userId = "user1";
            var tripId = 1;

            var trip = new Trip { Id = tripId, Name = "Trip to Paris" };

            _mockTripRepository.Setup(repo => repo.GetTripById(tripId)).ReturnsAsync(trip);

            // Act
            await _tripService.AddUserToTrip(userId, tripId);

            // Assert
            _mockTripRepository.Verify(repo => repo.GetTripById(tripId), Times.Once);
            _mockTripUserRepository.Verify(repo => repo.AddTripUser(It.IsAny<TripUser>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveUserFromTrip_RemovesUserSuccessfully()
        {
            // Arrange
            var userId = "user1";
            var tripId = 1;
            var trip = new Trip { Id = tripId, Name = "Trip to Paris" };
            var tripUser = new TripUser { UserId = userId, TripId = tripId };

            _mockTripRepository.Setup(repo => repo.GetTripById(tripId)).ReturnsAsync(trip);
            _mockTripUserRepository.Setup(repo => repo.GetTripUser(userId, tripId)).ReturnsAsync(tripUser);

            // Act
            await _tripService.RemoveUserFromTrip(userId, tripId);

            // Assert
            _mockTripRepository.Verify(repo => repo.GetTripById(tripId), Times.Once);
            _mockTripUserRepository.Verify(repo => repo.GetTripUser(userId, tripId), Times.Once);
            _mockTripUserRepository.Verify(repo => repo.RemoveTripUser(tripUser), Times.Once);
        }

        [TestMethod]
        public async Task RemoveTrip_DeletesTripSuccessfully()
        {
            // Arrange
            var tripId = 1;
            var trip = new Trip { Id = tripId, Name = "Trip to Paris" };

            _mockTripRepository.Setup(repo => repo.GetTripById(tripId)).ReturnsAsync(trip);

            // Act
            await _tripService.RemoveTrip(tripId);

            // Assert
            _mockTripRepository.Verify(repo => repo.GetTripById(tripId), Times.Once);
            _mockTripRepository.Verify(repo => repo.RemoveTrip(trip), Times.Once);
        }

        [TestMethod]
        public async Task GetTripDetails_ReturnsTripDetails()
        {
            // Arrange
            var tripId = 1;
            var trip = new Trip
            {
                Id = tripId,
                Name = "Trip to Paris",
                Destination = "Paris",
                Description = "A fun trip to Paris",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Users = new List<TripUser>
                {
                    new TripUser
                    {
                        UserId = "user1",
                        User = new User { FirstName = "John", LastName = "Doe" }
                    },
                    new TripUser
                    {
                        UserId = "user2",
                        User = new User { FirstName = "Jane", LastName = "Smith" }
                    }
                }
            };

            _mockTripRepository.Setup(repo => repo.GetTripByIdWithUsers(tripId)).ReturnsAsync(trip);

            // Act
            var result = await _tripService.GetTripDetails(tripId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tripId, result.Id);
            Assert.AreEqual(2, result.Participants.Count);
            Assert.AreEqual("John", result.Participants.First().FirstName);
        }

        [TestMethod]
        public async Task SetTripOwner_UpdatesTripOwner()
        {
            // Arrange
            var tripId = 1;
            var userId = "user1";
            var trip = new Trip { Id = tripId, Name = "Trip to Paris" };

            _mockTripRepository.Setup(repo => repo.GetTripById(tripId)).ReturnsAsync(trip);

            // Act
            await _tripService.SetTripOwner(userId, tripId);

            // Assert
            Assert.AreEqual(userId, trip.TripOwnerId);
            _mockTripRepository.Verify(repo => repo.UpdateTrip(trip), Times.Once);
        }
    }
}
