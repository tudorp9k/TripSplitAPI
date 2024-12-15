using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IConfiguration configuration;

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await authenticationService.Login(loginDto);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var request = new RegisterRequest
            {
                RegisterDto = registerDto,
                HttpRequest = Request
            };
            if (ModelState.IsValid)
            {
                await authenticationService.Register(request);
                return Ok();
            }

            return BadRequest();
        }

        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string id, [FromQuery] string confirmationToken)
        {
            if (ModelState.IsValid)
            {
                EmailConfirmationRequest request = new EmailConfirmationRequest()
                {
                    UserId = id,
                    ConfirmationToken = confirmationToken
                };

                await authenticationService.ConfirmEmail(request);

                return Ok();
                // var frontendAppUrl = configuration.GetSection("FrontendApp:Url");

                // return Redirect($"{frontendAppUrl.Value}/email-confirmation-page");
            }

            return BadRequest();
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var passwordResetRequestDto = new PasswordResetRequest
            {
                Email = email,
                HttpRequest = Request
            };

            if (ModelState.IsValid)
            {
                await authenticationService.RequestPasswordReset(passwordResetRequestDto);

                return Ok();

                // var frontendAppUrl = configuration.GetSection("FrontendApp:Url");

                // return Redirect($"{frontendAppUrl.Value}/password-reset-page");
            }

            return BadRequest();
        }

        [Route("ResetPassword")]
        [HttpGet]
        public async Task<IActionResult> PasswordReset([FromQuery] string userId, [FromQuery] string token)
        {
            return Ok();
            // var frontendAppUrl = configuration.GetSection("FrontendApp:Url");
            // return Redirect($"{frontendAppUrl.Value}/password-reset-page");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> PasswordReset(PasswordResetDto passwordResetDto)
        {
            if (ModelState.IsValid)
            {
                await authenticationService.PasswordReset(passwordResetDto);
                return Ok();
            }

            return BadRequest();
        }
    }
}