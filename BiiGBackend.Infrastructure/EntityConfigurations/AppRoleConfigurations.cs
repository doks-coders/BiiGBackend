using BiiGBackend.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiiGBackend.Infrastructure.EntityConfigurations
{
	public class AppRoleConfigurations : IEntityTypeConfiguration<AppRole>
	{
		public void Configure(EntityTypeBuilder<AppRole> builder)
		{
			builder.HasMany(u => u.UserRoles)
				.WithOne(u => u.AppRole)
				.HasForeignKey(u => u.RoleId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
