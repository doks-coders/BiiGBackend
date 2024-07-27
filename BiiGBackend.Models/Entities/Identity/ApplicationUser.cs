using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Identity;

namespace BiiGBackend.Models.Entities.Identity
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string Role { get; set; } = RoleConstants.Customer;
		public ICollection<AppUserRole> UserRoles { get; set; }
		public string Email { get; set; } = string.Empty;
		public string AccountType { get; set; } = string.Empty;
		public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
	}
}
