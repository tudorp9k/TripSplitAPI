using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService ?? throw new ArgumentNullException(nameof(invitationService));
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendInvitation([FromBody] InvitationDto invitationDto)
        {
            try
            {
                await _invitationService.SendInvitation(invitationDto.TripId, invitationDto.UserId);
                return Ok("Invitation sent successfully");
            }
            catch (Exception ex)
            {
                // If there's an overlap or any other error, return 400 with the message
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptInvitation([FromBody] InvitationDto invitationDto)
        {
            try
            {
                await _invitationService.AcceptInvitation(invitationDto.TripId, invitationDto.UserId);
                return Ok("Invitation accepted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectInvitation([FromBody] InvitationDto invitationDto)
        {
            try
            {
                await _invitationService.RejectInvitation(invitationDto.TripId, invitationDto.UserId);
                return Ok("Invitation rejected successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("invite-by-email")]
        public async Task<IActionResult> InviteUserByEmail([FromBody] InviteUserDto inviteUserDto)
        {
            try
            {
                await _invitationService.InviteUserByEmail(inviteUserDto.TripId, inviteUserDto.Email);
                return Ok("Invitation sent successfully to the user by email.");
            }
            catch (Exception ex)
            {
                // Overlap or user not found, etc.
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("get-user-invitations")]
        public async Task<IActionResult> GetUserInvitations(string userId)
        {
            var invitations = await _invitationService.GetInvitationsForUser(userId);
            return Ok(invitations);
        }
    }
}
