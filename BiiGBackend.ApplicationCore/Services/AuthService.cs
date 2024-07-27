using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.SharedModels.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorApplication.SharedModels.Validations.Auth;

namespace BiiGBackend.ApplicationCore.Services
{
	public class AuthService : IAuthService
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthService(ITokenService tokenService, UserManager<ApplicationUser> userManager)
		{
			_tokenService = tokenService;
			_userManager = userManager;
		}

		public async Task<ResponseModal> Login(LoginUserRequest request)
		{

			var validator = new LoginUserValidators();
			var res = await validator.ValidateAsync(request);
			if (!res.IsValid) throw new CustomException(res.Errors);

			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email) ?? throw new CustomException(ErrorCodes.UserDoesNotExist); ;

			var response = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!response) throw new CustomException(ErrorCodes.IncorrectPassword);

			var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
			return ResponseModal.Send(apiResponse);
		}

		public async Task<ResponseModal> Register(RegisterUserRequest request)
		{
			var validator = new RegisterUserValidators();
			var res = await validator.ValidateAsync(request);
			if (!res.IsValid) throw new CustomException(res.Errors);

			if (_userManager.Users.Any(u => u.Email == request.Email)) throw new CustomException(ErrorCodes.UserExist);
			ApplicationUser user = new();
			user.Email = request.Email;
			user.UserName = request.Email;
			user.AccountType = request.AccountType;
			var response = await _userManager.CreateAsync(user, request.Password);

			if (!response.Succeeded) throw new CustomException(response.Errors);
			var apiResponse = new AuthUserResponse() { Token = _tokenService.CreateToken(user), UserName = user.Email };
			return ResponseModal.Send(apiResponse);
		}


	}
}
