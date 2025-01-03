﻿using Microsoft.AspNetCore.Mvc;
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
            await _invitationService.SendInvitation(invitationDto.TripId, invitationDto.UserId);
            return Ok("Invitation sent successfully");
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptInvitation([FromBody] InvitationDto invitationDto)
        {
            await _invitationService.AcceptInvitation(invitationDto.TripId, invitationDto.UserId);
            return Ok("Invitation accepted successfully");
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectInvitation([FromBody] InvitationDto invitationDto)
        {
            await _invitationService.RejectInvitation(invitationDto.TripId, invitationDto.UserId);
            return Ok("Invitation rejected successfully");
        }

        [HttpPost("invite-by-email")]
        public async Task<IActionResult> InviteUserByEmail([FromBody] InviteUserDto inviteUserDto)
        {
            await _invitationService.InviteUserByEmail(inviteUserDto.TripId, inviteUserDto.Email);
            return Ok("Invitation sent successfully to the user by email.");
        }

        [HttpGet("get-user-invitations")]
        public async Task<IActionResult> GetUserInvitations(string userId)
        {
            var invitations = await _invitationService.GetInvitationsForUser(userId);
            return Ok(invitations);
        }
    }
}
