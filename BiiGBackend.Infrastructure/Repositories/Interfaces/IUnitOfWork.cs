﻿namespace BiiGBackend.Infrastructure.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		public IApplicationUserRepository User { get; }
		public IProductItemRepository Product { get; }
		public ICategoryRepository Category { get; }
		public IBrandRepository Brand { get; }
		public IShoppingCartItemRepository ShoppingCartItem { get; }

		public IOrderHeaderRepository OrderHeaders { get; }
		public IOrderItemsRepository OrderItems { get; }
		public ICollectionsRepository Collections { get; }
		Task<bool> Save();
	}
}
