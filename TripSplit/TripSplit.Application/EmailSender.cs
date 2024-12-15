using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration config)
        {
            configuration = config;
        }

        public async Task SendEmail(MimeMessage email)
        {
            var emailConfig = configuration.GetSection("EmailConfig");
            var username = emailConfig["EmailUsername"];
            var password = emailConfig["EmailPassword"];
            var emailHost = emailConfig["EmailHost"];
            var port = int.Parse(emailConfig["EmailPort"]);

            email.From.Add(MailboxAddress.Parse(username));

            var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(emailHost, port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(username, password);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
