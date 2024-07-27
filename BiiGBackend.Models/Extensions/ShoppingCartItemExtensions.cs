using BiiGBackend.Models.Entities;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Responses;

namespace BiiGBackend.Models.Extensions
{
	public static class ShoppingCartItemExtensions
	{
		public static IEnumerable<ShoppingCartItemResponse> ConvertShoppingCart(this IEnumerable<ShoppingCartItem> items)
		{
			return items.OrderByDescending(u => u.Created).Select(u =>
			{
				var photo = u.Product.ProductPhotos.FirstOrDefault(u => u.IsMain);
				var image = photo != null ? photo.Url : "";

				return new ShoppingCartItemResponse()
				{
					Id = u.Id,
					IsDiscounted = u.Product.IsDiscounted,
					ProductAmount = u.Count,
					ProductBrand = u.Product.ProductBrand.Name,
					ProductDisplayPrice = u.Count * u.Product.ProductRealPrice.GetProductDisplayPrice(u.Product.ProductDiscountPercent),
					ProductImage = image,
					ProductName = u.Product.ProductName,
					ProductRealPrice = u.Count * (double)u.Product.ProductRealPrice,
					Size = u.Size
				};

			}).ToList();
		}

		public static IEnumerable<OrderItem> ConvertToOrders(this IEnumerable<ShoppingCartItem> items, Guid orderHeaderId)
		{
			return items.OrderByDescending(u => u.Created).Select(u =>
			{
				var photo = u.Product.ProductPhotos.FirstOrDefault(u => u.IsMain);
				var image = photo != null ? photo.Url : "";

				return new OrderItem()
				{
					OrderHeaderId = orderHeaderId,
					OrderedRealPrice = (double)u.Product.ProductRealPrice,
					OrderedDisplayPrice = u.Product.ProductRealPrice.GetProductDisplayPrice(u.Product.ProductDiscountPercent),
					Size = u.Size,
					ApplicationUserId = u.ApplicationUserId,
					Count = u.Count,
					ProductId = u.ProductId,
				};

			}).ToList();
		}
	}
}
