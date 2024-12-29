using Microsoft.EntityFrameworkCore;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.DataAccess
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public TripRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Trip>> GetTripsByUserId(string userId)
        {
            return await _context.TripUsers
                .Where(tu => tu.UserId == userId)
                .Select(tu => tu.Trip)
                .ToListAsync();
        }

        public async Task<Trip> GetTripById(int tripId)
        {
            return await _context.Trips.FindAsync(tripId);
        }

        public async Task AddTrip(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrip(Trip trip)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTrip(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }
    }
}
