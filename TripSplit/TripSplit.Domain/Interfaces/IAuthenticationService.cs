using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> ConfirmEmail(EmailConfirmationRequest request);
        Task<LoginResult> Login(LoginDto loginDto);
        Task PasswordReset(PasswordResetDto passwordResetDto);
        Task Register(RegisterRequest registerRequest);
        Task RequestPasswordReset(PasswordResetRequest passwordResetRequest);
    }
}
