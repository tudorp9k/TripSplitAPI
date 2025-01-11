using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetTripsByUserId(string userId);
        Task<int> AddTrip(Trip trip);
        Task<Trip> GetTripById(int tripId);
        Task RemoveTrip(Trip trip);
        Task UpdateTrip(Trip trip);
        Task<Trip> GetTripByIdWithUsers(int tripId);

    }

}
