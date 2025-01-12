using Microsoft.AspNetCore.Http;
using Moq;
using System;
using TripSplit.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripSplit.Test
{
    [TestClass]
    public class HttpRequestExtensions_Test
    {
        [TestMethod]
        public void BaseUrl_ShouldReturnCorrectBaseUrl_WhenRequestIsNotNull()
        {
            // Arrange
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(req => req.Scheme).Returns("https");
            mockHttpRequest.Setup(req => req.Host).Returns(new HostString("example.com", 443));

            // Act
            var result = mockHttpRequest.Object.BaseUrl();

            // Assert
            Assert.AreEqual("https://example.com:443", result);
        }

        [TestMethod]
        public void BaseUrl_ShouldReturnBaseUrlWithoutPort_WhenPortIsDefault()
        {
            // Arrange
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(req => req.Scheme).Returns("https");
            mockHttpRequest.Setup(req => req.Host).Returns(new HostString("example.com", 80)); // Default port for http is 80, https is 443.

            // Act
            var result = mockHttpRequest.Object.BaseUrl();

            // Assert
            Assert.AreEqual("https://example.com", result); // Default port (80 or 443) should be omitted.
        }

        [TestMethod]
        public void BaseUrl_ShouldReturnNull_WhenRequestIsNull()
        {
            // Act
            string result = HttpRequestExtensions.BaseUrl(null);

            // Assert
            Assert.IsNull(result);
        }
    }
}
