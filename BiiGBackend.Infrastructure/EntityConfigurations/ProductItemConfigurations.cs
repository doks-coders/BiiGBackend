using BiiGBackend.Models.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiiGBackend.Infrastructure.EntityConfigurations
{
	public class ProductItemConfigurations : IEntityTypeConfiguration<ProductItem>
	{
		public void Configure(EntityTypeBuilder<ProductItem> builder)
		{
			builder.HasMany(u => u.ProductPhotos).WithOne(u => u.Product)
				.HasForeignKey(u => u.ProductId)
				.OnDelete(DeleteBehavior.NoAction);

			//builder.HasQueryFilter(u => u.isDeleted == false);
		}
	}
}
