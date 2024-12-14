using MimeKit;

namespace TripSplit.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(MimeMessage email);
    }
}
