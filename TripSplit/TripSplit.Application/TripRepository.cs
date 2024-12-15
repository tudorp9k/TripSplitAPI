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

        public async Task<IEnumerable<Trip>> GetTripsByUserId(int userId)
        {
            return await _context.Trips
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task AddTrip(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

    }
}
