using BiiGBackend.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiiGBackend.Infrastructure.EntityConfigurations
{
    public class OrderHeaderConfigurations : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasMany(u => u.OrderItems).WithOne(u => u.OrderHeader).HasForeignKey(u => u.OrderHeaderId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
