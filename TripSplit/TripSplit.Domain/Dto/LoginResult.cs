namespace TripSplit.Domain.Dto
{
    public class LoginResult
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
