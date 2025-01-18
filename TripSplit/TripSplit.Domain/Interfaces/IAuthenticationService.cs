using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> ConfirmEmail(EmailConfirmationRequest request);
        Task CreateAdmin();
        Task<bool> IsUserAdmin(string userId);
        Task<LoginResult> Login(LoginDto loginDto);
        Task PasswordReset(PasswordResetDto passwordResetDto);
        Task Register(RegisterRequest registerRequest);
        Task RequestPasswordReset(PasswordResetRequest passwordResetRequest);
    }
}
