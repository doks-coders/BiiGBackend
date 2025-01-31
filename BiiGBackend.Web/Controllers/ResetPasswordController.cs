using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
    [Route("reset-password")]
    public class ResetPasswordController : Controller
    {

        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public ResetPasswordController(IAuthService authService, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userid, string token)
        {
            if (await _authService.ConfirmEmail(userid, token))
            {
                var user = await _unitOfWork.User.GetItem(u => u.Id == Guid.Parse(userid));
                if (user != null)
                {
                    user.PasswordLock = false;
                    await _unitOfWork.Save();
                    return Redirect($"{GetUrl(_environment.IsDevelopment())}/set-password/{userid}");
                }

            }
            return Json(new
            {
                message = "Email could not be confirmed",
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
