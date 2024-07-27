using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiiGBackend.Models.Entities.Orders
{
	public class OrderItem : BaseEntity
	{
		public Guid ProductId { get; set; }
		[ForeignKey(nameof(ProductId))]
		public ProductItem Product { get; set; }

		public int Count { get; set; }
		public double OrderedRealPrice { get; set; }
		public double OrderedDisplayPrice { get; set; }

		public string? Size { get; set; }

		public Guid ApplicationUserId { get; set; }
		[ForeignKey(nameof(ApplicationUserId))]
		public ApplicationUser ApplicationUser { get; set; }

		public Guid OrderHeaderId { get; set; }
		public OrderHeader OrderHeader { get; set; }
	}
}
