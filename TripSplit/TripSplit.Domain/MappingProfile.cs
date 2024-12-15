using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Dto;

namespace TripSplit.Domain
{
    public static class MappingProfile
    {
        public static User RegisterDtoToUser(RegisterDto registerDto)
        {
            return new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };
        }

        public static UserDto UserToUserDto(User user) 
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public static User UserDtoToUser(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Email = userDto.Email,
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };
        }
    }
}
