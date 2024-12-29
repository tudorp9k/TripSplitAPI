using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IUserService
    {
        Task DeleteUser(string userId);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(string userId);
        Task UpdateUser(UserDto userdto);
    }
}
