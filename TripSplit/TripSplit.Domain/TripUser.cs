namespace TripSplit.Domain
{
    public class TripUser
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
