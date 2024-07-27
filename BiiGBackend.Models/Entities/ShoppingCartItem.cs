using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiiGBackend.Models.Entities
{
	public class ShoppingCartItem : BaseEntity
	{

		public Guid ProductId { get; set; }
		[ForeignKey(nameof(ProductId))]
		public ProductItem Product { get; set; }

		public int Count { get; set; }

		public string? Size { get; set; }

		public Guid ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
