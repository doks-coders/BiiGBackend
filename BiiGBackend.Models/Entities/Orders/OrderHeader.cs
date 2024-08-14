using BiiGBackend.Models.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiiGBackend.Models.Entities.Orders
{

	public class OrderHeader : BaseEntity
	{

		public Guid ApplicationUserId { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		public ApplicationUser ApplicationUser { get; set; }

		public DateTime? OrderDate { get; set; }
		public DateTime? ShippingDate { get; set; }

		public string? OrderStatus { get; set; }
		public string? PaymentStatus { get; set; }
		public string? TrackingNumber { get; set; }
		public string? Carrier { get; set; }

		public DateTime? PaymentDate { get; set; }
		public DateOnly? PaymentDueDate { get; set; }

		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }



		public string EmailAddress { get; set; }

		public string PhoneNumber { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string StreetAddress { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string PostalCode { get; set; }

		public double? USDToNairaRate { get; set; }
		public double? TotalInDollars { get; set; }	
		public double? TotalInNaira { get; set; }
		public double? LogisticsFee { get; set; }
		public List<OrderItem> OrderItems { get; set; } = new();


	}

}
