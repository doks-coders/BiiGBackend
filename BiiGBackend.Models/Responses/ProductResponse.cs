namespace BiiGBackend.Models.Responses
{
	public class ProductResponse
	{
		public Guid Id { get; set; }
		public string? ProductName { get; set; }

		public Guid? ProductBrandId { get; set; }
		public string ProductBrand { get; set; }

		public Guid? ProductCategoryId { get; set; }
		public string ProductCategory { get; set; }

		public string? ProductDescription { get; set; }
		public string? ProductSizes { get; set; }
		public double? ProductDisplayPrice { get; set; }

		public bool IsDiscounted { get; set; } = false;
		public double? ProductRealPrice { get; set; }
		public string? ProductImage { get; set; }
		public int? ProductStockAmount { get; set; }

		public bool? isFeatured { get; set; }
		public bool? isRecentlyAdded { get; set; }
	}
}
