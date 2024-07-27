using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpPost("place-order")]
		public async Task<ActionResult> PlaceOrder(OrderRequest request)
		{
			return await _orderService.PlaceOrder(request, User);
		}

		[HttpGet("confirm/{orderId}")]
		public async Task<ActionResult> Confirm(Guid orderId)
		{
			return await _orderService.ConfirmOrder(orderId);
		}

		[HttpGet("get-orders")]
		public async Task<ActionResult> GetOrders()
		{
			return await _orderService.GetAllOrders();
		}

		[HttpGet("get-order/{orderId}")]
		public async Task<ActionResult> GetOrder(Guid orderId)
		{
			return await _orderService.GetOrder(orderId);
		}

		[HttpGet("process-order/{orderId}")]
		public async Task<ActionResult> StartProcessing(Guid orderId)
		{
			return await _orderService.StartProcessing(orderId);
		}

		[HttpGet("ship-order/{orderId}")]
		public async Task<ActionResult> ShipOrder(Guid orderId)
		{
			return await _orderService.ShipOrder(orderId);
		}
		[HttpGet("approve-order/{orderId}")]
		public async Task<ActionResult> ApprovedOrder(Guid orderId)
		{
			return await _orderService.ApprovedOrder(orderId);
		}

		[HttpGet("cancel-order/{orderId}")]
		public async Task<ActionResult> CancelOrder(Guid orderId)
		{
			return await _orderService.CancelOrder(orderId);
		}


	}
}
