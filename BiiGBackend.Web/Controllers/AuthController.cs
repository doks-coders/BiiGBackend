using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{

    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest request)
        {
            return await _authService.Login(request);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
        {
            return await _authService.Register(request);
        }

        [HttpPost("register-complete")]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequestComplete request)
        {
            return await _authService.Register(request);
        }


        [HttpPost("set-password")]
        public async Task<ActionResult> SetPassword([FromBody] SetPasswordRequest setPassword)
        => await _authService.SetUserPassword(setPassword);

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        => await _authService.ForgotPassword(forgotPasswordRequest);


        [Authorize]
        [HttpGet("check")]
        public async Task<ActionResult> Check()
        {
            return Ok("Works");
        }
    }
}
