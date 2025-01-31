using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Extensions;
using BiiGBackend.Models.Requests;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace BiiGBackend.Web.Controllers
{

    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly APIContext _apiContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderService orderService, APIContext apiContext, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _apiContext = apiContext;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("place-order")]
        public async Task<ActionResult> PlaceOrder(OrderRequest request)
        {
            return await _orderService.PlaceOrder(request, User);
        }


        [HttpGet("confirm/{orderId}")]
        public async Task<ActionResult> Confirm(Guid orderId, [FromQuery] string paymentProvider)
        {
            return await _orderService.ConfirmOrder(orderId, paymentProvider);
        }


        [Authorize(Roles = RoleConstants.Admin)]
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



        [HttpGet("get-user-order")]
        public async Task<ActionResult> GetUserOrder()
        {
            return await _orderService.GetUserOrders(User.GetUserId());
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("process-order/{orderId}")]
        public async Task<ActionResult> StartProcessing(Guid orderId)
        {
            return await _orderService.StartProcessing(orderId);
        }


        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("ship-order/{orderId}")]
        public async Task<ActionResult> ShipOrder(Guid orderId)
        {
            return await _orderService.ShipOrder(orderId);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("approve-order/{orderId}")]
        public async Task<ActionResult> ApprovedOrder(Guid orderId)
        {
            return await _orderService.ApprovedOrder(orderId);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("cancel-order/{orderId}")]
        public async Task<ActionResult> CancelOrder(Guid orderId)
        {
            return await _orderService.CancelOrder(orderId);
        }

    }
}
