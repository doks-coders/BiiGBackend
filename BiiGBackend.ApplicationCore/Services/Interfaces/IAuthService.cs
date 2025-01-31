using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.StaticDefinitions.Constants;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModal> Login(LoginUserRequest request);
        Task<ResponseModal> Register(RegisterUserRequest request, string? role = RoleConstants.Customer);
        Task<ResponseModal> Register(RegisterUserRequestComplete request, string? role = RoleConstants.Customer);

        Task<bool> ConfirmEmail(string userId, string token);
        Task<ResponseModal> SetUserPassword(SetPasswordRequest passwordRequest);
        Task<ResponseModal> ForgotPassword(ForgotPasswordRequest request);
    }
}
