using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string TripOwnerId { get; set; }
        public User TripOwner { get; set; }

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public ICollection<TripUser> Users { get; set; } = new List<TripUser>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
