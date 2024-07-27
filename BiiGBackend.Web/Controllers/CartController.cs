using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
	public class CartController : BaseController
	{
		private IShoppingCartService _cartService;

		public CartController(IShoppingCartService cartService)
		{
			_cartService = cartService;
		}

		[HttpGet("get-cart-items")]
		public async Task<ActionResult> GetCartItems()
		{
			return await _cartService.GetCartItems(User);
		}

		[HttpPost("add-cart-item")]
		public async Task<ActionResult> AddCartItem(ShoppingCartItemRequest request)
		{
			return await _cartService.AddCartItem(request, User);
		}

		[HttpDelete("remove-cart-item/{cartId}")]
		public async Task<ActionResult> RemoveCartItem(Guid cartId)
		{
			return await _cartService.RemoveCartItem(cartId, User);
		}

		[HttpPut("modify-count")]
		public async Task<ActionResult> ModifyCount(CountRequest request)
		{
			return await _cartService.ModifyCount(request, User);
		}



	}
}
