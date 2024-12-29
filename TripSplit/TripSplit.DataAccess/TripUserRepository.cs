using TripSplit.DataAccess;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.DataAccess
{
    public class TripUserRepository : ITripUserRepository
    {
        private readonly ApplicationDbContext _context;

        public TripUserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTripUser(TripUser tripUser)
        {
            await _context.TripUsers.AddAsync(tripUser);
            await _context.SaveChangesAsync();
        }

        public async Task<TripUser> GetTripUser(string userId, int tripId)
        {
            var tripUser = await _context.TripUsers.FindAsync(userId, tripId);
            return tripUser;
        }

        public async Task RemoveTripUser(TripUser tripUser)
        {
            _context.TripUsers.Remove(tripUser);
            await _context.SaveChangesAsync();
        }
    }
}
