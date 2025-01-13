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

        // TripService.cs
        public async Task<int> CreateTrip(CreateTripDto createTripDto, string userId)
        {
            // 1) Convert DTO -> Domain
            var newTrip = MappingProfile.CreateTripDtoToTrip(createTripDto);

            // 2) Check if this user is already engaged in an overlapping trip
            //    We can fetch all trips for that user, then see if any overlap
            var userTrips = await tripRepository.GetTripsByUserId(userId);

            bool isOverlapping = userTrips.Any(t =>
                // Overlap check: if (newTrip.StartDate <= t.EndDate) && (newTrip.EndDate >= t.StartDate)
                // that indicates at least one day overlaps
                newTrip.StartDate <= t.EndDate && newTrip.EndDate >= t.StartDate
            );

            if (isOverlapping)
            {
                throw new Exception("You already have another trip that overlaps this time period.");
            }

            // 3) Create the new trip in the DB
            var tripId = await tripRepository.AddTrip(newTrip);

            // 4) Also attach the user as the trip owner or participant
            //    Example: set the TripOwner, or add them via TripUser:
            await tripUserRepository.AddTripUser(new TripUser
            {
                TripId = tripId,
                UserId = userId
            });

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
                StartDate = trip.StartDate.Date.ToShortDateString(),
                EndDate = trip.EndDate.Date.ToShortDateString(),
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
