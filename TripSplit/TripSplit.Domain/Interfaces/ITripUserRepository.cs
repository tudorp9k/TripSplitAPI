using TripSplit.Domain;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripUserRepository
    {
        Task AddTripUser(TripUser tripUser);
        Task<TripUser> GetTripUser(string userId, int tripId);
        Task RemoveTripUser(TripUser tripUser);
    }
}