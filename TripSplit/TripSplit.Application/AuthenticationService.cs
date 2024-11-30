using Microsoft.AspNetCore.Identity;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Exceptions;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;

        public AuthenticationService(UserManager<User> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new InvalidUserCredentialsException("User not found");
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                throw new InvalidUserCredentialsException("Invalid password");
            }

            var userDto = MappingProfile.UserToUserDto(user);
            return userDto;
        }

        public async Task Register(RegisterDto registerDto)
        {
            var existingEmail = await userManager.FindByEmailAsync(registerDto.Email);

            if (existingEmail != null)
            {
                throw new EmailTakenException();
            }

            var newUser = MappingProfile.RegisterDtoToUser(registerDto);

            var result = await userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new InvalidUserCredentialsException("User creation failed");
            }
        }
    }
}