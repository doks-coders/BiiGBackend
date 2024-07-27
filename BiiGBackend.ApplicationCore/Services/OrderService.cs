using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BiiGBackend.ApplicationCore.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStripePayment _stripePayment;
		private readonly IPaystackService _paystackService;

		public OrderService(IUnitOfWork unitOfWork, IStripePayment stripePayment, IPaystackService paystackService)
		{
			_unitOfWork = unitOfWork;
			_stripePayment = stripePayment;
			_paystackService = paystackService;
		}

		[Authorize]
		public async Task<ResponseModal> PlaceOrder(OrderRequest request, ClaimsPrincipal user)
		{
			var order = new OrderHeader()
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				PhoneNumber = request.PhoneNumber,
				PostalCode = request.PostalCode,
				StreetAddress = request.StreetAddress,
				Country = request.Country,
				State = request.State,
				City = request.City,
				OrderStatus = OrderStatus.StatusPending,
				PaymentStatus = PaymentStatus.PaymentStatusPending,
				EmailAddress = request.EmailAddress,
				TrackingNumber = new Random().Next(100000, 999999).ToString(),
				ApplicationUserId = user.GetUserId(),

			};
			await _unitOfWork.OrderHeaders.AddItem(order);
			var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == user.GetUserId(), includeProperties: "Product");
			var orderItems = cartItems.ConvertToOrders(order.Id);
			var totalAmount = orderItems.Sum(u => u.Count * u.OrderedDisplayPrice);
			await _unitOfWork.OrderItems.AddItems(orderItems);


			var payStackUrl = await _paystackService.InitializeTransaction(order.EmailAddress, totalAmount, order.Id);
			if (false)
			{
				var stripeUrl = await _stripePayment.InitialisePayment(order.Id, orderItems);
			}

			return ResponseModal.Send(payStackUrl);
		}



		public async Task<ResponseModal> ConfirmOrder(Guid orderId)
		{
			var verificationResponse = await _paystackService.VerifyTransaction(orderId);
			if (verificationResponse.Status != null) throw new CustomException("Payment Not Confirmed");
			var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
			order.PaymentStatus = PaymentStatus.PaymentStatusApproved;
			await _unitOfWork.Save();

			var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == order.ApplicationUserId, includeProperties: "Product");
			await _unitOfWork.ShoppingCartItem.DeleteItems(cartItems);

			var orderItemsResponse = order.OrderItems.ConvertToOrdersResponse();
			var orderResponse = order.GetOrderExtendedResponses(orderItemsResponse);

			return ResponseModal.Send(orderResponse);
		}

		public async Task<ResponseModal> DeleteOrder()
		{
			return ResponseModal.Send();
		}

		public async Task<ResponseModal> GetAllOrders()
		{
			var orderItems = await _unitOfWork.OrderHeaders.GetItems(u => u.Id != null, includeProperties: "ApplicationUser,OrderItems");
			var response = orderItems.OrderByDescending(u => u.Created).Select(e => new OrderResponse()
			{
				DateCreated = e.Created,
				EmailAddress = e.EmailAddress,
				FirstName = e.FirstName,
				LastName = e.LastName,
				Id = e.Id,
				TrackingNumber = e.TrackingNumber,
				TotalPrice = e.OrderItems.Sum(u => u.OrderedDisplayPrice)
			});
			return ResponseModal.Send(response);
		}

		public async Task<ResponseModal> GetOrder(Guid orderId)
		{
			var orderHeader = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);

			var orderItemsResponse = orderHeader.OrderItems.ConvertToOrdersResponse();

			var orderHeaderResponse = orderHeader.GetOrderExtendedResponses(orderItemsResponse);

			return ResponseModal.Send(orderHeaderResponse);
		}

		public async Task<ResponseModal> StartProcessing(Guid orderId)
		{
			var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
			order.OrderStatus = OrderStatus.StatusInProcess;
			return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusInProcess) : throw new CustomException("Unsuccessful");
		}



		public async Task<ResponseModal> ShipOrder(Guid orderId)
		{
			var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
			order.OrderStatus = OrderStatus.StatusShipped;
			return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusShipped) : throw new CustomException("Unsuccessful");
		}

		public async Task<ResponseModal> ApprovedOrder(Guid orderId)
		{
			var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
			order.OrderStatus = OrderStatus.StatusApproved;
			return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusApproved) : throw new CustomException("Unsuccessful");
		}


		public async Task<ResponseModal> CancelOrder(Guid orderId)
		{
			var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
			order.OrderStatus = OrderStatus.StatusCancelled;
			return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusCancelled) : throw new CustomException("Unsuccessful");

		}

		/**
		 public async Task<ResponseModal> PlaceOrder(OrderRequest request, ClaimsPrincipal user)
		{
			var order = new OrderHeader()
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				PhoneNumber = request.PhoneNumber,
				PostalCode = request.PostalCode,
				StreetAddress = request.StreetAddress,
				Country = request.Country,
				State = request.State,
				City = request.City,
				OrderStatus = OrderStatus.StatusPending,
				PaymentStatus = PaymentStatus.PaymentStatusPending,
				EmailAddress = request.EmailAddress,
				TrackingNumber = new Random().Next(100000, 999999).ToString(),
				ApplicationUserId = user.GetUserId(),

			};
			await _unitOfWork.OrderHeaders.AddItem(order);
			var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == user.GetUserId(), includeProperties: "Product");
			var orderItems = cartItems.ConvertToOrders(order.Id);

			await _unitOfWork.OrderItems.AddItems(orderItems);

			await _unitOfWork.ShoppingCartItem.DeleteItems(cartItems);

			return ResponseModal.Send();
		}
		 */


	}
}
