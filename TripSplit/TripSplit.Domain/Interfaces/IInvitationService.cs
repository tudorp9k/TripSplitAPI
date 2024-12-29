using System.Collections.Generic;
using System.Threading.Tasks;
using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IInvitationService
    {
        Task SendInvitation(int tripId, string userId);
        Task AcceptInvitation(int tripId, string userId);
        Task RejectInvitation(int tripId, string userId);
        Task InviteUserByEmail(int tripId, string email);
        Task<List<InvitationDto>> GetInvitationsForUser(string userId);
    }
}
