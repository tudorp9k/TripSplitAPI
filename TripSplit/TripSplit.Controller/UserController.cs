using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await userService.GetUserById(userId);
            return Ok(result);
        }

        [HttpPatch("update-user")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            await userService.UpdateUser(userDto);
            return Ok();
        }
    }
}
