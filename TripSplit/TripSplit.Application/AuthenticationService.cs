using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Exceptions;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailService emailService;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, ITokenService tokenService, IConfiguration configuration)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task<LoginResult> Login(LoginDto loginDto)
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

            var token = await tokenService.CreateTokenAsync(user);
            var userDto = MappingProfile.UserToUserDto(user);
            var loginResult = new LoginResult
            {
                Token = token,
                User = userDto
            };
            return loginResult;
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var registerDto = registerRequest.RegisterDto;
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

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
            await emailService.SendConfirmationEmail(newUser, confirmationToken, registerRequest.HttpRequest.BaseUrl());
        }

        public async Task<string> ConfirmEmail(EmailConfirmationRequest request)
        {
            var decodedUserId = HttpUtility.UrlDecode(request.UserId);
            var decodedConfirmationToken = HttpUtility.UrlDecode(request.ConfirmationToken);

            var user = await userManager.FindByIdAsync(decodedUserId);

            if (user == null)
            {
                throw new InvalidUserCredentialsException("User not found");
            }

            var confirmEmail = await userManager.ConfirmEmailAsync(user, decodedConfirmationToken);

            if (!confirmEmail.Succeeded)
            {
                throw new InvalidUserCredentialsException("Email confirmation failed");
            }

            return decodedUserId;
        }

        public async Task RequestPasswordReset(PasswordResetRequest passwordResetRequest)
        {
            var user = await userManager.FindByEmailAsync(passwordResetRequest.Email);

            if (user == null)
            {
                throw new InvalidUserCredentialsException("User not found");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var baseUrl = passwordResetRequest.HttpRequest.BaseUrl();
            await emailService.SendPasswordResetEmail(user, token, baseUrl);
        }

        public async Task PasswordReset(PasswordResetDto passwordResetDto)
        {
            var decodedUserId = HttpUtility.UrlDecode(passwordResetDto.UserId);
            var decodedToken = HttpUtility.UrlDecode(passwordResetDto.Token);

            var user = await userManager.FindByIdAsync(decodedUserId);

            if (user == null)
            {
                throw new InvalidUserCredentialsException("User not found");
            }

            var result = await userManager.ResetPasswordAsync(user, decodedToken, passwordResetDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new InvalidUserCredentialsException("Password reset failed");
            }
        }


        public async Task CreateAdmin()
        {
            var adminConfig = configuration.GetSection("AdminCredentials");
            var adminEmail = adminConfig["Email"];
            var adminPassword = adminConfig["Password"];

            var newAdmin = new User
            {
                Email = adminEmail,
                UserName = adminEmail,
                FirstName = "Admin",
                LastName = "Admin",
            };

            var result = await userManager.CreateAsync(newAdmin, adminPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Admin creation failed");
            }

            var admin = await userManager.FindByEmailAsync(adminEmail);
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        public async Task<bool> IsUserAdmin(string userId)
        {
            var admin = await userManager.FindByIdAsync(userId);
            if (admin == null)
            {
                throw new Exception("User not found");
            }

            var result = await userManager.IsInRoleAsync(admin, "Admin");
            return result;
        }
    }
}