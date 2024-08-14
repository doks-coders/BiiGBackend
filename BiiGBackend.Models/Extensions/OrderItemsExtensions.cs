using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Responses;

namespace BiiGBackend.Models.Extensions
{
	public static class OrderItemsExtensions
	{
		public static IEnumerable<OrderItemResponse> ConvertToOrdersResponse(this IEnumerable<OrderItem> items)
		{
			return items.OrderByDescending(u => u.Created).Select(u =>
			{
				var photo = u.Product.ProductPhotos.FirstOrDefault(u => u.IsMain);
				var image = photo != null ? photo.Url : "";

				return new OrderItemResponse()
				{
					Size = u.Size,
					IsDiscounted = u.Product.IsDiscounted,
					Id = u.Id,
					ProductId=u.ProductId,
					ProductAmount = u.Count,
					ProductDisplayPrice = u.OrderedDisplayPrice.ToString(),
					ProductRealPrice = u.OrderedRealPrice.ToString(),
					ProductBrand = u.Product.ProductBrand.Name,
					ProductName = u.Product.ProductName,
					ProductImage = image
				};

			}).ToList();
		}
	}
}
