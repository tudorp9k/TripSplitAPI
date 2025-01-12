using Moq;
using MimeKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripSplit.Application;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;
using System.Threading.Tasks;

namespace TripSplit.Test
{
    [TestClass]
    public class EmailService_Test
    {
        private Mock<IEmailSender> _mockEmailSender;
        private Mock<IEmailTemplateBuilder> _mockEmailTemplateBuilder;
        private EmailService _emailService;

        [TestInitialize]
        public void SetUp()
        {
            // Mock dependencies
            _mockEmailSender = new Mock<IEmailSender>();
            _mockEmailTemplateBuilder = new Mock<IEmailTemplateBuilder>();

            // Initialize EmailService with mocked dependencies
            _emailService = new EmailService(_mockEmailSender.Object, _mockEmailTemplateBuilder.Object);
        }

        [TestMethod]
        public async Task SendPasswordResetEmail_ShouldCallSendEmail()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var resetToken = "resetToken";
            var baseUrl = "http://example.com";

            // Mock email template builder to return a BodyBuilder
            _mockEmailTemplateBuilder.Setup(x => x.PasswordResetMailTemplate(user, resetToken, baseUrl))
                                     .Returns(new BodyBuilder());

            // Act
            await _emailService.SendPasswordResetEmail(user, resetToken, baseUrl);

            // Assert: Verify SendEmail was called
            _mockEmailSender.Verify(x => x.SendEmail(It.IsAny<MimeMessage>()), Times.Once);
        }

        [TestMethod]
        public async Task SendConfirmationEmail_ShouldCallSendEmail()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var confirmationToken = "confirmationToken";
            var baseUrl = "http://example.com";

            // Mock email template builder to return a BodyBuilder
            _mockEmailTemplateBuilder.Setup(x => x.ConfirmationMailTemplate(user, confirmationToken, baseUrl))
                                     .Returns(new BodyBuilder());

            // Act
            await _emailService.SendConfirmationEmail(user, confirmationToken, baseUrl);

            // Assert: Verify SendEmail was called
            _mockEmailSender.Verify(x => x.SendEmail(It.IsAny<MimeMessage>()), Times.Once);
        }

        [TestMethod]
        public void CreateEmail_ShouldReturnCorrectEmail()
        {
            // Arrange
            var to = "test@example.com";
            var subject = "Subject";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Body of the email";

            // Act
            var email = _emailService.CreateEmail(to, subject, bodyBuilder);

            // Assert
            Assert.AreEqual(to, email.To.ToString());
            Assert.AreEqual(subject, email.Subject);
            Assert.AreEqual(bodyBuilder.TextBody, email.Body.ToString());
        }
    }
}
