using BiiGBackend.Models.Entities.StaticItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiiGBackend.Models.Entities.Product
{
	public class ProductItem : BaseEntity
	{
		public bool ProductDetailsUploaded { get; set; } = false;
		public string? ProductName { get; set; }

		public Guid? ProductBrandId { get; set; }

		[ForeignKey(nameof(ProductBrandId))]
		public Brand ProductBrand { get; set; }


		public Guid? ProductCategoryId { get; set; }

		[ForeignKey(nameof(ProductCategoryId))]
		public Category ProductCategory { get; set; }

		public string? ProductDescription { get; set; }
		public string? ProductSizes { get; set; }
		public int? ProductDiscountPercent { get; set; }
		public bool IsDiscounted { get; set; } = false;
		public double? ProductRealPrice { get; set; }
		public string? ProductImage { get; set; }
		public int? ProductStockAmount { get; set; }

		public bool? isFeatured { get; set; }
		public bool? isRecentlyAdded { get; set; }

		public List<Photo>? ProductPhotos { get; set; } = new();
	}
}

