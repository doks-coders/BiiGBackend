using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiiGBackend.Models.DTOs
{
	public class CreateWishlistItemDto
	{
		public Guid ProductId { get; set; }
	}

	public class WishlistItemDto
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string ProductName { get; set; }
		public bool isReleased { get; set; }
		public DateTime AddedOn { get; set; }
		public string ProductImage { get;set; }
		public List<string> UserEmails { get; set; } = new();
	}

	public class AddUserToWishlistDto
	{
		public Guid WishlistItemId { get; set; }
		public string Email { get; set; }
	}
}
