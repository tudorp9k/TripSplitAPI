namespace TripSplit.Domain
{
    public class Invitation
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsDenied { get; set; }
    }
}
