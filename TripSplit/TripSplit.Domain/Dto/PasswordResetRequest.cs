using Microsoft.AspNetCore.Http;

namespace TripSplit.Domain.Dto
{
    public class PasswordResetRequest
    {
        public string Email { get; set; }
        public HttpRequest HttpRequest { get; set; }
    }
}
