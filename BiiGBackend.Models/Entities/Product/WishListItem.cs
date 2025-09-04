using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiiGBackend.Models.Entities.Product
{
	public class WishListItem: BaseEntity
	{
		public bool isReleased { get; set; } = false;
		public DateTime AddedOn { get; set; } = DateTime.UtcNow;
		[ForeignKey(nameof(ProductId))]
		public Guid ProductId { get; set; } 
		public ProductItem Product { get; set; }
		public List<InterestedUser> Users { get; set; } = [];
	}

	///modelBuilder.Entity<InstituteAuditTrail>(e => e.OwnsMany(f => f.Data).ToJson());



}
