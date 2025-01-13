using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TripDetailDto.cs
namespace TripSplit.Domain.Dto
{
    public class TripDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public List<TripParticipantDto> Participants { get; set; } = new List<TripParticipantDto>();
    }
}

// TripParticipantDto.cs
namespace TripSplit.Domain.Dto
{
    public class TripParticipantDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

