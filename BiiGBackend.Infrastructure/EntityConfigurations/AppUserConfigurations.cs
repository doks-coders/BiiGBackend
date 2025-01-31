using BiiGBackend.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiiGBackend.Infrastructure.EntityConfigurations
{
    public class AppUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(u => u.UserRoles)
                .WithOne(u => u.AppUser)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.HasMany(u => u.ShoppingCartItems)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey(u => u.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
