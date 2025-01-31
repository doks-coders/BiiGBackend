using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.SharedModels.Enums;
using BiiGBackend.StaticDefinitions.Constants;
using ChatUpdater.ApplicationCore.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json;
using TutorApplication.SharedModels.Validations.Auth;

namespace BiiGBackend.ApplicationCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _senderEmail;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        public AuthService(IConfiguration configuration, ITokenService tokenService, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<AppRole> roleManager, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, IWebHostEnvironment environment)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _senderEmail = configuration["EmailSettings:SenderEmail"];
            _environment = environment;
        }

        public async Task<ResponseModal> Login(LoginUserRequest request)
        {

            var validator = new LoginUserValidators();
            var res = await validator.ValidateAsync(request);
            if (!res.IsValid) throw new CustomException(res.Errors);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower()) ?? throw new CustomException(ErrorCodes.UserDoesNotExist); ;

            if (user.PasswordLock != null && user.PasswordLock == false) throw new CustomException(ErrorCodes.IncorrectPassword);
            var response = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!response) throw new CustomException(ErrorCodes.IncorrectPassword);

            var roles = await _userManager.GetRolesAsync(user);

            var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email, Roles = roles.ToArray() };
            return ResponseModal.Send(apiResponse);
        }

        public async Task<ResponseModal> Register(RegisterUserRequest request, string? role = RoleConstants.Customer)
        {
            var validator = new RegisterUserValidators();
            var res = await validator.ValidateAsync(request);
            if (!res.IsValid) throw new CustomException(res.Errors);

            if (_userManager.Users.Any(u => u.Email.ToLower() == request.Email.ToLower())) throw new CustomException(ErrorCodes.UserExist);
            ApplicationUser user = new();
            user.Email = request.Email;
            user.UserName = request.Email;
            user.AccountType = role;
            user.PasswordLock = false;
            var response = await _userManager.CreateAsync(user);

            if (response.Succeeded)
            {
                await SendSetPasswordEmail(user);
                return ResponseModal.Send(true);
            }

            if (response.Errors.FirstOrDefault(i => i.Code == "DuplicateUserName") != null)
            {
                throw new CustomException("User name exists");
            }

            if (!response.Succeeded) throw new CustomException(response.Errors);
            var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
            return ResponseModal.Send(apiResponse);
        }



        public async Task<ResponseModal> Register(RegisterUserRequestComplete request, string? role = RoleConstants.Customer)
        {
            var validator = new RegisterUserValidatorsComplete();
            var res = await validator.ValidateAsync(request);
            if (!res.IsValid) throw new CustomException(res.Errors);

            if (_userManager.Users.Any(u => u.Email.ToLower() == request.Email.ToLower())) throw new CustomException("User not verified");
            ApplicationUser user = new();
            user.Email = request.Email;
            user.UserName = request.Email;
            user.Role = role;
            user.AccountType = role;
            user.PasswordLock = true;
            var response = await _userManager.CreateAsync(user, request.Password);


            if (response.Errors.FirstOrDefault(i => i.Code == "DuplicateUserName") != null)
            {
                throw new CustomException("User name exists");
            }

            if (!response.Succeeded) throw new CustomException(response.Errors);


            Console.WriteLine(JsonSerializer.Serialize(_roleManager.Roles.ToList()));

            IdentityResult resR = await _userManager.AddToRoleAsync(user, role);
            if (!resR.Succeeded) throw new CustomException(response.Errors);

            var roles = await _userManager.GetRolesAsync(user);
            var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email, Roles = roles.ToArray() };
            return ResponseModal.Send(apiResponse);
        }




        private string GetUrl()
        {

            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";

        }


        public async Task<bool> ConfirmEmail(string userId, string token)
        {

            var decodedUserId = Guid.Parse(userId);

            var user = await _unitOfWork.User.GetItem(u => u.Id == decodedUserId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _unitOfWork.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ResponseModal> SetUserPassword(SetPasswordRequest passwordRequest)
        {
            var setPasswordRequestValidator = new SetPasswordRequestValidator();
            var validation = await setPasswordRequestValidator.ValidateAsync(passwordRequest);
            if (!validation.IsValid) throw new CustomException(validation.Errors);

            var user = await _unitOfWork.User.GetItem(u => u.Id == passwordRequest.UserId);

            if (user == null) throw new CustomException(ErrorCodes.UserDoesNotExist);

            if (passwordRequest.Password != passwordRequest.ConfirmPassword) throw new CustomException(ErrorCodes.IncorrectPassword);

            if (user.EmailConfirmed == true && user.PasswordLock == false)
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.RemovePasswordAsync(user);
                    var res = await _userManager.AddPasswordAsync(user, passwordRequest.Password);
                    if (!res.Succeeded) throw new CustomException(ErrorCodes.IncorrectPassword);


                }
                else
                {
                    var res = await _userManager.AddPasswordAsync(user, passwordRequest.Password);
                    if (!res.Succeeded) throw new CustomException(ErrorCodes.IncorrectPassword);

                }

                user.PasswordLock = true;
                await _unitOfWork.Save();

                var token = _tokenService.CreateToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email, Roles = roles.ToArray() };
                return ResponseModal.Send(apiResponse);
            }

            throw new CustomException(ErrorCodes.IncorrectPassword);
        }

        public async Task<ResponseModal> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _unitOfWork.User.GetItem(u => u.Email.ToLower() == request.Email.ToLower() && u.Email.ToLower() != "admin@godanddevil.death");

            if (user != null)
            {
                await SendForgotPasswordEmail(user);
                return ResponseModal.Send(true);
            }
            else
            {
                throw new CustomException(ErrorCodes.UserDoesNotExist);
            }
            /*
			 Send Email,
			Verfiy Email,
			Reset Password
			 */
        }


        private async Task SendSetPasswordEmail(ApplicationUser applicationUser)
        {
            if (string.IsNullOrEmpty(applicationUser.Email))
                throw new CustomException("No Email");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var verifyUrl = QueryHelpers.AddQueryString(
                $"{GetUrl()}/verify-email", "token", token);

            verifyUrl = QueryHelpers.AddQueryString(
                verifyUrl, "userid", applicationUser.Id.ToString());


            Log.Information(verifyUrl);

            string subject = "Set-Password";
            string htmlMessage = @$"<p>
   Thanks for registering with us. <br>
   To set a password for your account please click 
<a href=""{verifyUrl}"" target=""_blank"" rel=""noopener noreferrer"">Set Password</a>. </p>
   Regards,
";
            try
            {

                await _emailSender.SendEmailAsync(fromEmail: _senderEmail, toEmail: applicationUser.Email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new CustomException(ex.Message);
            }

        }


        private async Task SendForgotPasswordEmail(ApplicationUser applicationUser)
        {
            if (string.IsNullOrEmpty(applicationUser.Email))
                throw new CustomException("No Email");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var verifyUrl = QueryHelpers.AddQueryString(
                $"{GetUrl()}/reset-password", "token", token);

            verifyUrl = QueryHelpers.AddQueryString(
                verifyUrl, "userid", applicationUser.Id.ToString());


            Log.Information(verifyUrl);

            string subject = "Reset-Password";
            string htmlMessage = @$"<p>
   Please, use this link to reset the password for your account please click <br>
<a href=""{verifyUrl}"" target=""_blank"" rel=""noopener noreferrer"">Reset Password</a>. <br>
   Regards </p>,
";
            try
            {
                await _emailSender.SendEmailAsync(fromEmail: _senderEmail, toEmail: applicationUser.Email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new CustomException(ex.Message);
            }

        }

    }
}
