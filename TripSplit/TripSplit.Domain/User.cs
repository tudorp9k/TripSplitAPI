using Microsoft.AspNetCore.Identity;

namespace TripSplit.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}