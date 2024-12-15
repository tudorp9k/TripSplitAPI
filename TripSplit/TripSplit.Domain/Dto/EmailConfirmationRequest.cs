namespace TripSplit.Domain.Dto
{
    public class EmailConfirmationRequest
    {
        public string UserId { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
