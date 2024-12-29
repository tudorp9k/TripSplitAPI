using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Dto
{
    public class AddUserToTripRequest
    {
        public int TripId { get; set; }
        public string UserId { get; set; }
    }
}
