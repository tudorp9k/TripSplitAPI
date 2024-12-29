using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripService
    {
        Task AddUserToTrip(string userId, int tripId);
        Task CreateTrip(CreateTripDto trip);
        Task<IEnumerable<TripDto>> GetUserTrips(string userId);
        Task RemoveTrip(int tripId);
    }
}