using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface IShoppingCartService
	{
		Task<ResponseModal> AddCartItem(ShoppingCartItemRequest request, ClaimsPrincipal user);

		Task<ResponseModal> RemoveCartItem(Guid cartId, ClaimsPrincipal user);

		Task<ResponseModal> ModifyCount(CountRequest request, ClaimsPrincipal user);

		Task<ResponseModal> GetCartItems(ClaimsPrincipal user);
	}
}
