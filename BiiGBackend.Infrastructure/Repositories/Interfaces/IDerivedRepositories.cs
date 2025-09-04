using BiiGBackend.Models.Entities;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.Entities.StaticItems;

namespace BiiGBackend.Infrastructure.Repositories.Interfaces
{

    public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserById(Guid id);
    }

    public interface IProductItemRepository : IBaseRepository<ProductItem>
    {
        Task<bool> DeleteOne(Guid id);
    }

    public interface ICategoryRepository : IBaseRepository<Category>
    {

    }

    public interface IBrandRepository : IBaseRepository<Brand>
    {

    }

    public interface IShoppingCartItemRepository : IBaseRepository<ShoppingCartItem>
    {
        Task<bool> DeleteOne(Guid id);
        Task<IEnumerable<ShoppingCartItem>> GetCartItems(Guid userId);
    }


    public interface IOrderHeaderRepository : IBaseRepository<OrderHeader>
    {
        Task<OrderHeader> GetOrderHeader(Guid orderId);
    }

    public interface IOrderItemsRepository : IBaseRepository<OrderItem>
    {

    }

    public interface ICollectionsRepository : IBaseRepository<Collection>
    {

    }

    public interface IStaticDataRepository : IBaseRepository<StaticData>
    {

    }
	public interface IWishListItemRepository : IBaseRepository<WishListItem>
	{

	}

	

}


