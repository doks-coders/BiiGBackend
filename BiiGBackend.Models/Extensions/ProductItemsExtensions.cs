using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.Responses;

namespace BiiGBackend.Models.Extensions
{
	public static class ProductItemsExtensions
	{
		public static IEnumerable<ProductResponse> GetProductResponse(this IEnumerable<ProductItem> products)
		{
			return products.Select(u =>
			{
				var photo = u.ProductPhotos.FirstOrDefault(u => u.IsMain);
				var image = photo != null ? photo.Url : "";

				return new ProductResponse()
				{
					Id = u.Id,
					isFeatured = u.isFeatured,
					isRecentlyAdded = u.isRecentlyAdded,
					IsDiscounted = u.IsDiscounted,
					ProductBrand = u.ProductBrand.Name,
					ProductBrandId = u.ProductBrandId,
					ProductCategory = u.ProductCategory.Name,
					ProductRealPrice = u.ProductRealPrice,
					ProductName = u.ProductName,
					ProductDescription = u.ProductDescription,
					ProductCategoryId = u.ProductCategoryId,
					ProductDisplayPrice = u.ProductRealPrice.GetProductDisplayPrice(u.ProductDiscountPercent),
					ProductImage = image,
					ProductSizes = u.ProductSizes,
					ProductStockAmount = u.ProductStockAmount,

				};
			});
		}


		public static IEnumerable<ProductResponseExtended> GetExtendedProductResponse(this IEnumerable<ProductItem> products)
		{
			return products.OrderByDescending(u => u.Created).Select(u =>
			{
				var photo = u.ProductPhotos.FirstOrDefault(u => u.IsMain);
				var image = photo != null ? photo.Url : "";

				return new ProductResponseExtended()
				{
					Id = u.Id,
					isFeatured = u.isFeatured,
					isRecentlyAdded = u.isRecentlyAdded,
					IsDiscounted = u.IsDiscounted,
					ProductBrand = u.ProductBrand.Name,
					ProductBrandId = u.ProductBrandId,
					ProductCategory = u.ProductCategory.Name,
					ProductRealPrice = u.ProductRealPrice,
					ProductName = u.ProductName,
					ProductDescription = u.ProductDescription,
					ProductCategoryId = u.ProductCategoryId,
					ProductDisplayPrice = u.ProductRealPrice.GetProductDisplayPrice(u.ProductDiscountPercent),
					ProductDiscountPercent = u.ProductDiscountPercent,
					ProductImage = image,
					ProductSizes = u.ProductSizes,
					ProductStockAmount = u.ProductStockAmount,
					ProductPhotos = u.ProductPhotos.Select(u => new PhotoResponse() { Id = u.Id, IsMain = u.IsMain, Url = u.Url, ProductId = u.ProductId }).ToList(),
				};
			});
		}


		public static double GetProductDisplayPrice(this double? ProductRealPrice, double? ProductDiscountPercent)
		{
			return (double)ProductRealPrice / (100 / (double)(100 - ProductDiscountPercent));
		}
	}
}
