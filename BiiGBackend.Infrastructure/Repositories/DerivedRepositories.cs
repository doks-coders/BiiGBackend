using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.Entities.StaticItems;
using Microsoft.EntityFrameworkCore;

namespace BiiGBackend.Infrastructure.Repositories
{
	public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
	{
		public ApplicationUserRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<ApplicationUser> GetUserById(Guid id)
		{
			return await GetItem(u => u.Id == id);
		}
	}

	public class ProductItemRepository : BaseRepository<ProductItem>, IProductItemRepository
	{
		public ProductItemRepository(ApplicationDbContext context) : base(context)
		{

		}

		public async Task<bool> DeleteOne(Guid id)
		{
			var product = await GetItem(u => u.Id == id);
			await DeleteItem(product);
			return await _context.SaveChangesAsync() > 0;
		}
	}


	public class BrandRepository : BaseRepository<Brand>, IBrandRepository
	{
		public BrandRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
	public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext context) : base(context)
		{

		}
	}

	public class ShoppingCartItemRepository : BaseRepository<ShoppingCartItem>, IShoppingCartItemRepository
	{
		private readonly ApplicationDbContext _context;
		public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
		public async Task<bool> DeleteOne(Guid id)
		{
			var item = await GetItem(u => u.Id == id);
			await DeleteItem(item);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<ShoppingCartItem>> GetCartItems(Guid userId)
		{
			return await _dbSet.Where(u => u.ApplicationUserId == userId).Include(f => f.ApplicationUser)
				.Include(f => f.Product)
				.ThenInclude(s => s.ProductBrand)
				.Include(f => f.Product)
				.ThenInclude(s => s.ProductCategory)
				.Include(f => f.Product)
				.ThenInclude(s => s.ProductPhotos)
				.ToListAsync();

		}
	}

	public class OrderHeaderRepository : BaseRepository<OrderHeader>, IOrderHeaderRepository
	{
		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
		{

		}

		public async Task<OrderHeader> GetOrderHeader(Guid orderId)
		{
			return await _dbSet.Where(u => u.Id == orderId).Include(f => f.OrderItems)
				.ThenInclude(f => f.Product)
				.ThenInclude(g => g.ProductPhotos)
				.Include(f => f.OrderItems)
				.ThenInclude(f => f.Product)
				.ThenInclude(g => g.ProductBrand)
				.FirstOrDefaultAsync();

		}
	}

	public class OrderItemsRepository : BaseRepository<OrderItem>, IOrderItemsRepository
	{
		public OrderItemsRepository(ApplicationDbContext context) : base(context)
		{

		}
	}

	public class CollectionRepository : BaseRepository<Collection>, ICollectionsRepository
	{
		public CollectionRepository(ApplicationDbContext context) : base(context)
		{

		}
	}




}
