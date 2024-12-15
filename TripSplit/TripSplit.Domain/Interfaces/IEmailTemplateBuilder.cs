using MimeKit;

namespace TripSplit.Domain.Interfaces
{
    public interface IEmailTemplateBuilder
    {
        BodyBuilder ConfirmationMailTemplate(User user, string confirmationToken, string baseUrl);
        BodyBuilder PasswordResetMailTemplate(User user, string resetToken, string baseUrl);
    }
}
