using Microsoft.AspNetCore.Identity;
using TripSplit.Domain;
using System;
using TripSplit.Domain.Interfaces;
using TripSplit.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TripSplit.Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserDto> GetUserById(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var userDto = MappingProfile.UserToUserDto(user);
            return userDto;
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var user = MappingProfile.UserDtoToUser(userDto);
            var userToUpdate = await userManager.FindByIdAsync(user.Id);

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;

            var result = await userManager.UpdateAsync(userToUpdate);
            if (!result.Succeeded)
            {
                throw new Exception("User update failed");
            }
        }

        public async Task<GetAllUsersResponse> GetAllUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = users.Select(user => MappingProfile.UserToUserDto(user));
            var response = new GetAllUsersResponse
            {
                Users = userDtos
            };
            return response;
        }

        public async Task DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("User deletion failed");
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var userDto = MappingProfile.UserToUserDto(user);
            return userDto;
        }
    }
}
