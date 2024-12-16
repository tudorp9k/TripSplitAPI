using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Interfaces;
using TripSplit.Domain;

namespace TripSplit.Application
{
    public class TripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<Trip>> GetUserTrips(string userId)
        {
            return await _tripRepository.GetTripsByUserId(userId);
        }

        public async Task CreateTrip(Trip trip)
        {
            await _tripRepository.AddTrip(trip);
        }

    }
}
