using Microsoft.AspNetCore.Identity;
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
        private readonly IEmailService emailService;

        public AuthenticationService(UserManager<User> userManager, IEmailService emailService)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
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
    }
}