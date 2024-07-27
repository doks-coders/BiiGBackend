using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseModal> Login(LoginUserRequest request);
		Task<ResponseModal> Register(RegisterUserRequest request);
	}
}
