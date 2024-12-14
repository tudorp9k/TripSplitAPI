namespace TripSplit.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(User user, string confirmationToken, string baseUrl);
    }
}
