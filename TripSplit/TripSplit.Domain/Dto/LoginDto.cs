using System.ComponentModel.DataAnnotations;

namespace TripSplit.Domain.Dto
{
    public class LoginDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
