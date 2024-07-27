using System.Security.Claims;

namespace BiiGBackend.Models.Extensions
{
	public static class ClaimsExtensions
	{
		public static Guid GetUserId(this ClaimsPrincipal user)
		{
			return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
		}

	}
}
