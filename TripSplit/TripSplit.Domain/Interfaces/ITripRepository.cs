using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetTripsByUserId(int userId);
        Task AddTrip(Trip trip);

    }

}
