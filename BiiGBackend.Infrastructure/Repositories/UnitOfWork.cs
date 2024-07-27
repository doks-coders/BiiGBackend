using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Infrastructure.Repositories.Interfaces;

namespace BiiGBackend.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		public IApplicationUserRepository User { get; }
		public IProductItemRepository Product { get; }
		public ICategoryRepository Category { get; }
		public IBrandRepository Brand { get; }
		public IOrderHeaderRepository OrderHeaders { get; }
		public IOrderItemsRepository OrderItems { get; }

		public IShoppingCartItemRepository ShoppingCartItem { get; }
		public ICollectionsRepository Collections { get; }


		private readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			User = new ApplicationUserRepository(context);
			Product = new ProductItemRepository(context);
			Category = new CategoryRepository(context);
			Brand = new BrandRepository(context);
			ShoppingCartItem = new ShoppingCartItemRepository(context);
			OrderHeaders = new OrderHeaderRepository(context);
			OrderItems = new OrderItemsRepository(context);
			Collections = new CollectionRepository(context);


		}

		public async Task<bool> Save()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
