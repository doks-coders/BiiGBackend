using BiiGBackend.ApplicationCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatUpdater.Controllers
{

    [Route("verify-email")]
    public class VerifyEmailController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public VerifyEmailController(IAuthService authService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userid, string token)
        {
            if (await _authService.ConfirmEmail(userid, token))
            {
                return Redirect($"{GetUrl(_environment.IsDevelopment())}/set-password/{userid}");
            }
            return Json(new
            {
                message = "User could not be confirmed",
                success = false
            });
        }

        private string GetUrl(bool isDev = false)
        {
            if (isDev)
            {
                return "https://localhost:4200";
            }
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";

        }
    }
}
