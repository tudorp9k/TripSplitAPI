using TripSplit.Domain;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripUserRepository
    {
        Task AddTripUser(TripUser tripUser);
    }
}