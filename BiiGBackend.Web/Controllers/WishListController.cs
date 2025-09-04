using BiiGBackend.ApplicationCore.Services;
using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
	public class WishListController(IWishListService _wishListService) : BaseController
	{
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return await _wishListService.GetAllAsync();
		}

		// GET: api/WishList/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			return await _wishListService.GetByIdAsync(id);
		}

		// POST: api/WishList
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateWishlistItemDto dto)
		{
			return await _wishListService.CreateWishlistItemAsync(dto);
		}

		// DELETE: api/WishList/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return await _wishListService.DeleteWishlistItemAsync(id);
		}

		// POST: api/WishList/add-user
		[HttpPost("add-user")]
		public async Task<IActionResult> AddUser([FromBody] AddUserToWishlistDto dto)
		{
			return await _wishListService.AddUserToWishlistAsync(dto);
		}
	}
}
