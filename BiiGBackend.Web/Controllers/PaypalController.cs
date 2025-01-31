using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.SharedModels;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace BiiGBackend.Web.Controllers
{
    public class PaypalController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly APIContext _apiContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public PaypalController(IOrderService orderService, APIContext apiContext, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _apiContext = apiContext;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("execute-paypal/{id}")]
        public async Task<ActionResult> ExecutePaypal(Guid id, [FromQuery] string paymentId, string token, string PayerID, string cancel)
        {
            if (cancel == "true") throw new CustomException("Cancelled");
            var apiContext = _apiContext;
            var paymentExecution = new PaymentExecution() { payer_id = PayerID };
            var payment = new Payment() { id = paymentId };

            var order = await _unitOfWork.OrderHeaders.GetItem(u => u.Id == id);

            // Execute the payment
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            if (executedPayment.state.ToLower() != "approved")
            {
                throw new CustomException("Failed Payment");
            }
            order.PaymentStatus = PaymentStatus.PaymentStatusApproved;

            var cartItems = await _unitOfWork.ShoppingCartItem.GetItems(u => u.ApplicationUserId == order.ApplicationUserId, includeProperties: "Product");
            await _unitOfWork.ShoppingCartItem.DeleteItems(cartItems);
            return Ok(order.Id);
        }


        private string GetUrl(bool isDev = false)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (isDev)
            {
                return $"{request.Scheme}://localhost:4100";
            }
            else
            {

                return $"{request.Scheme}://{request.Host}{request.PathBase}";
            }

        }

    }
}
