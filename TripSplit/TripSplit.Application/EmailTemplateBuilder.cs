using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Web;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        private readonly IConfiguration configuration;
        public EmailTemplateBuilder(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public BodyBuilder ConfirmationMailTemplate(User user, string confirmationToken, string baseUrl)
        {
            var templateFilePath = Path.Combine("..", "TripSplit.Application", "Templates", "EmailTemplate.html");

            UriBuilder verificationLink = BuildEmailVerificationLink(user, confirmationToken, baseUrl);

            var htmlContent = File.ReadAllText(templateFilePath);
            htmlContent = htmlContent.Replace("[User's Name]", user.FirstName)
                                     .Replace("[Verification Link]", $"{verificationLink}");

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = htmlContent
            };

            return bodyBuilder;
        }

        public BodyBuilder PasswordResetMailTemplate(User user, string resetToken, string baseUrl)
        {
            var templateFilePath = Path.Combine("..", "TripSplit.Application", "Templates", "PasswordResetTemplate.html");

            UriBuilder resetLink = BuildPasswordResetLink(user, resetToken, baseUrl);

            var htmlContent = File.ReadAllText(templateFilePath);
            htmlContent = htmlContent.Replace("[User's Name]", user.FirstName)
                                     .Replace("[Reset Link]", $"{resetLink}");

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = htmlContent
            };

            return bodyBuilder;
        }

        private UriBuilder BuildPasswordResetLink(User user, string resetToken, string baseUrl)
        {
            var frontendAppUrl = configuration.GetSection("FrontendApp:Url").Value;
            var uriBuilder = new UriBuilder($"{frontendAppUrl}/ResetPassword");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            string encodedUserId = HttpUtility.UrlEncode(user.Id);
            string encodedToken = HttpUtility.UrlEncode(resetToken);

            query["id"] = encodedUserId;
            query["resetToken"] = encodedToken;

            uriBuilder.Query = query.ToString();
            return uriBuilder;
        }

        private UriBuilder BuildEmailVerificationLink(User user, string confirmationToken, string baseUrl)
        {
            var frontendAppUrl = configuration.GetSection("FrontendApp:Url").Value;
            var uriBuilder = new UriBuilder($"{frontendAppUrl}/AccountValidated");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Query = query.ToString();
            return uriBuilder;
        }
    }
}
