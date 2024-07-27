using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Responses;

namespace BiiGBackend.Models.Extensions
{
	public static class OrderHeaderExtensions
	{
		public static OrderExtendedResponse GetOrderExtendedResponses(this OrderHeader orderHeader, IEnumerable<OrderItemResponse> orderItemsResponse)
		{
			return new OrderExtendedResponse()
			{
				Id = orderHeader.Id,
				OrderStatus = orderHeader.OrderStatus,
				PaymentStatus = orderHeader.PaymentStatus,
				PhoneNumber = orderHeader.PhoneNumber,
				EmailAddress = orderHeader.EmailAddress,
				FirstName = orderHeader.FirstName,
				DateCreated = orderHeader.Created,
				LastName = orderHeader.LastName,

				StreetAddress = orderHeader.StreetAddress,

				Country = orderHeader.Country,

				City = orderHeader.City,

				State = orderHeader.State,

				PostalCode = orderHeader.PostalCode,
				OrderItems = orderItemsResponse.ToList(),
				TrackingNumber = orderHeader.TrackingNumber

			};


		}
	}
}
