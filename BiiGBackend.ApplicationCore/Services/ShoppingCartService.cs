using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.SharedModels.Enums;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModal> GetCartItems(ClaimsPrincipal user)
        {
            try
            {
                Guid id = user.GetUserId();
                var items = await _unitOfWork.ShoppingCartItem.GetCartItems(id);
                var response = items.ConvertShoppingCart();
                return ResponseModal.Send(response);
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorCodes.CartItemNotRetrievable);
            }
        }

        public async Task<ResponseModal> AddCartItem(ShoppingCartItemRequest request, ClaimsPrincipal user)
        {
            Guid id;

            try
            {
                id = user.GetUserId();
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorCodes.NotAuthenticatedLogin);
            }

            var shoppingCartItem = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == id && u.ProductId == request.ProductId);

            if (shoppingCartItem != null && shoppingCartItem.Any(u => u.Size == request.Size)) throw new CustomException(ErrorCodes.CartItemExist);

            var item = new ShoppingCartItem()
            {
                ProductId = request.ProductId,
                Count = request.Count < 1 ? 1 : request.Count,
                Size = request.Size,
                ApplicationUserId = id
            };
            await _unitOfWork.ShoppingCartItem.AddItem(item);
            return ResponseModal.Send(item);

        }

        public async Task<ResponseModal> ModifyCount(CountRequest request, ClaimsPrincipal user)
        {
            var item = await _unitOfWork.ShoppingCartItem.GetItem(u => u.Id == request.ShoppingCartId, includeProperties: "Product");
            if (item == null) throw new CustomException(ErrorCodes.CourseDoesNotExist);


            if (request.Mode == "Increment")
            {
                item.Count++;
            }

            if (item.Count > 1)
            {
                if (request.Mode == "Decrement")
                {
                    item.Count--;
                }
            }

            var response = new CountResponse()
            {
                Count = item.Count,
                Id = item.Id,
                Price = item.Count * item.Product.ProductRealPrice.GetProductDisplayPrice(item.Product.ProductDiscountPercent),
                RealPrice = item.Count * (double)item.Product.ProductRealPrice

            };

            await _unitOfWork.Save();

            return ResponseModal.Send(response);
        }

        public async Task<ResponseModal> RemoveCartItem(Guid cartId, ClaimsPrincipal user)
        {
            await _unitOfWork.ShoppingCartItem.DeleteOne(cartId);
            return ResponseModal.Send(cartId);
        }
    }
}
