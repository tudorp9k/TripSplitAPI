using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Web;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
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
            var uriBuilder = new UriBuilder($"{baseUrl}api/Authentication/ResetPassword/");
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
            var uriBuilder = new UriBuilder($"{baseUrl}api/Authentication/ConfirmEmail/");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            string encodedUserId = HttpUtility.UrlEncode(user.Id);
            string encodedToken = HttpUtility.UrlEncode(confirmationToken);
            query["id"] = encodedUserId;
            query["confirmationToken"] = encodedToken;

            uriBuilder.Query = query.ToString();
            return uriBuilder;
        }
    }
}
