using BiiGBackend.Models.Entities.Identity;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(ApplicationUser user);
	}
}
