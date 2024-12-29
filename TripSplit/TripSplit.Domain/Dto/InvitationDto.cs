namespace TripSplit.Domain.Dto
{
    public class InvitationDto
    {
        public int TripId { get; set; }
        public string UserId { get; set; }
        public bool IsDenied { get; set; }
    }

    public class InviteUserDto
    {
        public int TripId { get; set; }
        public string Email { get; set; }
    }
}
