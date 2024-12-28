using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.DataAccess;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class InvitationService : IInvitationService
    {
        private readonly ApplicationDbContext _context;

        public InvitationService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SendInvitation(int tripId, string userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
                throw new Exception("Trip not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var existingInvitation = await _context.Invitations
                .FindAsync(tripId, userId);
            if (existingInvitation != null)
                throw new Exception("Invitation already exists");

            var invitation = new Invitation
            {
                TripId = tripId,
                UserId = userId,
                IsDenied = false
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptInvitation(int tripId, string userId)
        {
            var invitation = await _context.Invitations
                .FindAsync(tripId, userId);
            if (invitation == null)
                throw new Exception("Invitation not found");

            _context.Invitations.Remove(invitation);

            var tripUser = new TripUser
            {
                TripId = tripId,
                UserId = userId
            };
            _context.TripUsers.Add(tripUser);

            await _context.SaveChangesAsync();
        }

        public async Task RejectInvitation(int tripId, string userId)
        {
            var invitation = await _context.Invitations
                .FindAsync(tripId, userId);
            if (invitation == null)
                throw new Exception("Invitation not found");

            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
