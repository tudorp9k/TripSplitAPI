using Microsoft.EntityFrameworkCore;
using TripSplit.DataAccess;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public TripRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTripsByUserId(string userId)
        {
            return await _context.TripUsers
                .Where(tu => tu.UserId == userId)
                .Select(tu => tu.Trip)
                .ToListAsync();
        }

        public async Task AddTrip(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

    }
}
