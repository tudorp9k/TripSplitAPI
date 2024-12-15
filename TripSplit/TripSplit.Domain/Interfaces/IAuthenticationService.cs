﻿using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> ConfirmEmail(EmailConfirmationRequest request);
        Task<UserDto> Login(LoginDto loginDto);
        Task PasswordReset(PasswordResetDto passwordResetDto);
        Task Register(RegisterRequest registerRequest);
        Task RequestPasswordReset(PasswordResetRequest passwordResetRequest);
    }
}
