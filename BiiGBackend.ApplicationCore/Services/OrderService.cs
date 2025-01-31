using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.Orders;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;
using System.Text.Json;

namespace BiiGBackend.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStripePayment _stripePayment;
        private readonly IPaystackService _paystackService;
        private readonly IStaticItemsService _staticItemsService;
        private readonly IPaypalService _paypalService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork, IStripePayment stripePayment, IPaystackService paystackService, IStaticItemsService staticItemsService, IPaypalService paypalService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _stripePayment = stripePayment;
            _paystackService = paystackService;
            _staticItemsService = staticItemsService;
            _paypalService = paypalService;
            _httpContextAccessor = httpContextAccessor;
        }


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
                PaymentProvider = request.PaymentProvider

            };
            await _unitOfWork.OrderHeaders.AddItem(order);
            var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == user.GetUserId() && u.Product.isDeleted == false, includeProperties: "Product");
            var orderItems = cartItems.ConvertToOrders(order.Id);
            var totalAmount = orderItems.Sum(u => u.Count * u.OrderedDisplayPrice);
            var staticData = await _staticItemsService.GetStaticDataFromDatabase();

            var retrievedOrder = await _unitOfWork.OrderHeaders.GetItem(u => u.Id == order.Id);


            if (staticData.OverseasTransport != null)
            {
                retrievedOrder.LogisticsFee = (double)staticData.OverseasTransport;
                totalAmount += (double)staticData.OverseasTransport;
            }

            retrievedOrder.TotalInDollars = totalAmount;

            if (staticData.USDtoNaira != null)
            {
                retrievedOrder.USDToNairaRate = (double)staticData.USDtoNaira;
                totalAmount = totalAmount * (double)staticData.USDtoNaira;
            }


            retrievedOrder.TotalInNaira = totalAmount;
            await _unitOfWork.Save();

            await _unitOfWork.OrderItems.AddItems(orderItems);

            if (request.PaymentProvider == "PAYSTACK")
            {
                return await InitialisePaystackPayment((double)retrievedOrder.TotalInNaira, retrievedOrder);
            }
            if (request.PaymentProvider == "PAYPAL")
            {
                return await InitialisePaypalPayment((double)retrievedOrder.TotalInDollars, orderItems, retrievedOrder);
            }
            throw new CustomException("No Payment Provider added");
        }

        public async Task<ResponseModal> InitialisePaystackPayment(double totalAmount, OrderHeader order)
        {
            var payStackUrl = await _paystackService.InitializeTransaction(order.EmailAddress, totalAmount, order.Id);

            return ResponseModal.Send(payStackUrl);
        }


        public async Task<ResponseModal> InitialisePaypalPayment(double totalAmount, IEnumerable<OrderItem> orderItems, OrderHeader order)
        {
            try
            {
                var redirect = $"{GetUrl()}/paypal-execute/{order.Id}";
                var payment = _paypalService.CreatePayment(redirect, totalAmount, orderItems, (double)order.LogisticsFee);

                Log.Information("Payment Created");
                Log.Information(JsonSerializer.Serialize(payment));

                var approvalUrl = payment.links.FirstOrDefault(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));
                return ResponseModal.Send(approvalUrl.href);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        /*
		 Scenario one

		User buys product that has been deleted
		 */

        public async Task<ResponseModal> ConfirmOrder(Guid orderId, string paymentProvider)
        {
            var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);

            if (paymentProvider == "PAYSTACK")
            {
                var verificationResponse = await _paystackService.VerifyTransaction(orderId);
                if (verificationResponse.Status != null) throw new CustomException("Payment Not Confirmed");

                order.PaymentStatus = PaymentStatus.PaymentStatusApproved;
                await _unitOfWork.Save();
                var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == order.ApplicationUserId, includeProperties: "Product");
                await _unitOfWork.ShoppingCartItem.DeleteItems(cartItems);

                return await PaymentApproved(order);
            }

            if (paymentProvider == "PAYPAL")
            {
                if (order.PaymentStatus != PaymentStatus.PaymentStatusApproved) throw new CustomException("Payment Not Approved");
                return await PaymentApproved(order);
            }

            throw new CustomException("");
        }

        public async Task<ResponseModal> PaymentApproved(OrderHeader order)
        {
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
            var orderItems = await _unitOfWork.OrderHeaders.GetItems(u => u.Id != null && u.PaymentStatus != PaymentStatus.PaymentStatusPending, includeProperties: "ApplicationUser,OrderItems");
            var response = orderItems.OrderByDescending(u => u.Created).Select(e => new OrderResponse()
            {
                DateCreated = e.Created,
                EmailAddress = e.EmailAddress,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Id = e.Id,
                TrackingNumber = e.TrackingNumber,
                TotalPrice = e.TotalInDollars != null ? (double)e.TotalInDollars : 0,
            });
            return ResponseModal.Send(response);
        }


        public async Task<ResponseModal> GetUserOrders(Guid userId)
        {
            var orderItems = await _unitOfWork.OrderHeaders.GetItems(u => u.ApplicationUserId == userId && u.PaymentStatus != PaymentStatus.PaymentStatusPending, includeProperties: "ApplicationUser,OrderItems");
            var response = orderItems.OrderByDescending(u => u.Created).Select(e => new OrderResponse()
            {
                DateCreated = e.Created,
                EmailAddress = e.EmailAddress,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Id = e.Id,
                TrackingNumber = e.TrackingNumber,
                TotalPrice = e.TotalInDollars != null ? (double)e.TotalInDollars : 0,
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

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> StartProcessing(Guid orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
            order.OrderStatus = OrderStatus.StatusInProcess;
            return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusInProcess) : throw new CustomException("Unsuccessful");
        }


        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> ShipOrder(Guid orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
            order.OrderStatus = OrderStatus.StatusShipped;
            return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusShipped) : throw new CustomException("Unsuccessful");
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> ApprovedOrder(Guid orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
            order.OrderStatus = OrderStatus.StatusApproved;
            return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusApproved) : throw new CustomException("Unsuccessful");
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseModal> CancelOrder(Guid orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetOrderHeader(orderId);
            order.OrderStatus = OrderStatus.StatusCancelled;
            return await _unitOfWork.Save() ? ResponseModal.Send(OrderStatus.StatusCancelled) : throw new CustomException("Unsuccessful");

        }

        private string GetUrl(bool isDev = false)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (isDev)
            {
                return $"{request.Scheme}://localhost:4200";
            }
            else
            {

                return $"{request.Scheme}://{request.Host}{request.PathBase}";
            }

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
