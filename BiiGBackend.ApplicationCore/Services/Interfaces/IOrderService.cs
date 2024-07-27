using BiiGBackend.Models.Requests;
using BiiGBackend.Models.SharedModels;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
	public interface IOrderService
	{
		Task<ResponseModal> PlaceOrder(OrderRequest request, ClaimsPrincipal user);
		Task<ResponseModal> ConfirmOrder(Guid orderId);

		Task<ResponseModal> StartProcessing(Guid orderId);
		Task<ResponseModal> ShipOrder(Guid orderId);
		Task<ResponseModal> ApprovedOrder(Guid orderId);
		Task<ResponseModal> CancelOrder(Guid orderId);

		Task<ResponseModal> GetAllOrders();
		Task<ResponseModal> GetOrder(Guid orderId);
	}
}
