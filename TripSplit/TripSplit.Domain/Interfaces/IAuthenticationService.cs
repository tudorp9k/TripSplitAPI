using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task Register(RegisterDto registerDto);
    }
}
