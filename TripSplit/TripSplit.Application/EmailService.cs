using MimeKit;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateBuilder emailTemplateBuilder;

        public EmailService(IEmailSender emailSender, IEmailTemplateBuilder emailTemplateBuilder)
        {
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.emailTemplateBuilder = emailTemplateBuilder ?? throw new ArgumentNullException(nameof(emailTemplateBuilder));
        }

        public async Task SendPasswordResetEmail(User user, string resetToken, string baseUrl)
        {
            BodyBuilder bodyBuilder = emailTemplateBuilder.PasswordResetMailTemplate(user, resetToken, baseUrl);

            var email = CreateEmail(user.Email, "Password Reset Email", bodyBuilder);

            await SendEmail(email);
        }

        public async Task SendConfirmationEmail(User user, string confirmationToken, string baseUrl)
        {
            BodyBuilder bodyBuilder = emailTemplateBuilder.ConfirmationMailTemplate(user, confirmationToken, baseUrl);

            var email = CreateEmail(user.Email, "Confirmation Email", bodyBuilder);

            await SendEmail(email);
        }
        private async Task SendEmail(MimeMessage email)
        {
            await emailSender.SendEmail(email);
        }

        public MimeMessage CreateEmail(string to, string subject, BodyBuilder body)
        {
            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = body.ToMessageBody();

            return email;
        }
    }
}
