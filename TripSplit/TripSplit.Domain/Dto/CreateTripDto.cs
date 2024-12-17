namespace TripSplit.Domain.Dto
{
    public class CreateTripDto
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
