namespace TripSplit.Domain.Dto
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
