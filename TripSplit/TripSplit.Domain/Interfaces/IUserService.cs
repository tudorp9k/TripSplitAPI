using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(string userId);
        Task UpdateUser(UserDto userdto);
    }
}
