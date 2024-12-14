using Microsoft.AspNetCore.Http;

namespace TripSplit.Domain.Dto
{
    public class RegisterRequest
    {
        public RegisterDto RegisterDto { get; set; }
        public HttpRequest HttpRequest { get; set; }
    }
}
