using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.DTOs;
using BiiGBackend.Models.Entities.Product;
using BiiGBackend.Models.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiiGBackend.ApplicationCore.Services
{
    public class WishListService(IUnitOfWork unitOfWork) : IWishListService
	{
		public async Task<ResponseModal> CreateWishlistItemAsync(CreateWishlistItemDto dto)
		{
			var product = await unitOfWork.Product.GetItem(p => p.Id == dto.ProductId);
			if (product == null)
				return ResponseModal.Send("Product not found", HttpStatusCode.NotFound);

			var exists = await unitOfWork.WishListItems.GetItem(w => w.ProductId == dto.ProductId);
			if (exists != null)
				return ResponseModal.Send("Item already exists in wishlist", HttpStatusCode.Conflict);

			var wishlist = new WishListItem
			{
				ProductId = dto.ProductId
			};

			var result = await unitOfWork.WishListItems.AddItem(wishlist);
			return ResponseModal.Send(result ? "Wishlist item created" : "Failed to create item", result ? HttpStatusCode.Created : HttpStatusCode.InternalServerError);
		}

		public async Task<ResponseModal> GetAllAsync()
		{
			var items = await unitOfWork.WishListItems.GetItems(w => true, "Product,Users");

			var response = items.Select(w => new WishlistItemDto
			{
				Id = w.Id,
				ProductId = w.ProductId,
				ProductName = w.Product?.ProductName,
				isReleased = w.isReleased,
				AddedOn = w.AddedOn,
				UserEmails = w.Users?.Select(u => u.Email).ToList() ?? []
			}).ToList();

			return ResponseModal.Send(response);
		}

		public async Task<ResponseModal> GetByIdAsync(Guid id)
		{
			var item = await unitOfWork.WishListItems.GetItem(w => w.Id == id, "Product,Users");

			if (item == null)
				return ResponseModal.Send("Wishlist item not found", HttpStatusCode.NotFound);

			var product = await unitOfWork.Product.GetItem(p => p.Id == item.ProductId, includeProperties: "ProductPhotos,ProductBrand,ProductCategory");
			var photo = product?.ProductPhotos?.FirstOrDefault(p => p.IsMain);
			var image = photo?.Url ?? "";

			var response = new WishlistItemDto
			{
				Id = item.Id,
				ProductId = item.ProductId,
				ProductName = item.Product?.ProductName,
				isReleased = item.isReleased,
				AddedOn = item.AddedOn,
				ProductImage = image,
				UserEmails = item.Users?.Select(u => u.Email).ToList() ?? []
			};

			return ResponseModal.Send(response);
		}

		public async Task<ResponseModal> DeleteWishlistItemAsync(Guid id)
		{
			var item = await unitOfWork.WishListItems.GetItem(w => w.Id == id);
			if (item == null)
				return ResponseModal.Send("Wishlist item not found", HttpStatusCode.NotFound);

			var result = await unitOfWork.WishListItems.DeleteItem(item);
			return ResponseModal.Send(result ? "Wishlist item deleted" : "Failed to delete item", result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
		}

		public async Task<ResponseModal> AddUserToWishlistAsync(AddUserToWishlistDto dto)
		{
			var wishlist = await unitOfWork.WishListItems.GetItem(w => w.Id == dto.WishlistItemId, "Users");

			if (wishlist == null)
				return ResponseModal.Send("Wishlist item not found", HttpStatusCode.NotFound);

			var emailMatch = wishlist.Users.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));
			if (emailMatch)
				return ResponseModal.Send("User already added to this wishlist", HttpStatusCode.Conflict);

			wishlist.Users.Add(new InterestedUser
			{
				Email = dto.Email
			});

			var result = await unitOfWork.Save();
			return ResponseModal.Send(result ? "User added successfully" : "Failed to add user", result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
		}
	}
}
