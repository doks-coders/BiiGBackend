using BiiGBackend.Models.Entities;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.Entities.StaticItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BiiGBackend.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<StaticData> StaticDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
