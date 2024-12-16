using Microsoft.AspNetCore.Identity;

namespace TripSplit.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public ICollection<TripUser> Trips { get; set; } = new List<TripUser>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();

        public ICollection<Trip> OwnedTrips { get; set; } = new List<Trip>();
    }
}