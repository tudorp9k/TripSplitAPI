using Microsoft.AspNetCore.Http;

namespace TripSplit.Domain.Dto
{
    public class RegisterDto
    {
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Password { get; init; }
    }
}
