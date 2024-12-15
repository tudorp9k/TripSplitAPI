namespace TripSplit.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(User user, string confirmationToken, string baseUrl);
        Task SendPasswordResetEmail(User user, string resetToken, string baseUrl);
    }
}
