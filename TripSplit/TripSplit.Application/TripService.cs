using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class TripService : ITripService
    {
        private readonly ITripRepository tripRepository;
        private readonly ITripUserRepository tripUserRepository;

        public TripService(ITripRepository tripRepository, ITripUserRepository tripUserRepository)
        {
            this.tripRepository = tripRepository ?? throw new ArgumentNullException(nameof(tripRepository));
            this.tripUserRepository = tripUserRepository ?? throw new ArgumentNullException(nameof(tripUserRepository));
        }

        public async Task<IEnumerable<TripDto>> GetUserTrips(string userId)
        {
            var userTrips = await tripRepository.GetTripsByUserId(userId);
            var userTripsDto = userTrips.Select(t => MappingProfile.TripToTripDto(t));
            return userTripsDto;
        }

        public async Task<int> CreateTrip(CreateTripDto createTripDto)
        {
            var trip = MappingProfile.CreateTripDtoToTrip(createTripDto);
            var tripId = await tripRepository.AddTrip(trip);
            return tripId;
        }

        public async Task AddUserToTrip(string userId, int tripId)
        {
            var trip = await tripRepository.GetTripById(tripId);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            var userTrip = new TripUser
            {
                TripId = tripId,
                UserId = userId
            };

            await tripUserRepository.AddTripUser(userTrip);
        }

        public async Task RemoveUserFromTrip(string userId, int tripId)
        {
            var trip = await tripRepository.GetTripById(tripId);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            var userTrip = await tripUserRepository.GetTripUser(userId, tripId);
            if (userTrip == null)
            {
                throw new Exception("User not found in trip");
            }

            await tripUserRepository.RemoveTripUser(userTrip);
        }

        public async Task RemoveTrip(int tripId)
        {
            var trip = await tripRepository.GetTripById(tripId);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            await tripRepository.RemoveTrip(trip);
        }

        public async Task<TripDetailDto> GetTripDetails(int tripId)
        {
            // 1. Load the Trip from the repository with the Users included
            var trip = await tripRepository.GetTripByIdWithUsers(tripId);
            if (trip == null)
                throw new Exception("Trip not found");

            // 2. Create a TripDetailDto from the loaded Trip
            var tripDetailDto = new TripDetailDto
            {
                Id = trip.Id,
                Name = trip.Name,
                Destination = trip.Destination,
                Description = trip.Description,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Participants = trip.Users
                    .Select(tu => new TripParticipantDto
                    {
                        UserId = tu.UserId,
                        FirstName = tu.User.FirstName,
                        LastName = tu.User.LastName
                    })
                    .ToList()
            };

            return tripDetailDto;
        }

        public async Task SetTripOwner(string userId, int tripId)
        {
            var trip = await tripRepository.GetTripById(tripId);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            trip.TripOwnerId = userId;
            await tripRepository.UpdateTrip(trip);
        }
    }
}
