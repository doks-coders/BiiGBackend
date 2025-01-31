using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Entities.Orders;
using Microsoft.AspNetCore.Http;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace BiiGBackend.ApplicationCore.Services
{
    public class StripePayment : IStripePayment
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public StripePayment(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> InitialisePayment(Guid OrderHeaderId, IEnumerable<OrderItem> orderItems)
        {

            //its a customer, so let's make payment
            string domain = $"{GetUrl()}";
            SessionCreateOptions options = new SessionCreateOptions
            {
                SuccessUrl = $"{domain}/api/order/confirm/{OrderHeaderId}",
                CancelUrl = $"{domain}/api/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (OrderItem cart in orderItems)
            {
                SessionLineItemOptions lineItemOptions = new()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)cart.OrderedDisplayPrice * 100, //20.50 cents => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = cart.Product.ProductName,
                        }
                    },

                    Quantity = cart.Count,
                };

                options.LineItems.Add(lineItemOptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            var orderHeader = await _unitOfWork.OrderHeaders.GetItem(u => u.Id == OrderHeaderId);
            orderHeader.SessionId = session.Id;
            orderHeader.PaymentIntentId = session.PaymentIntentId;

            await _unitOfWork.Save();

            return session.Url;
        }



        private string GetUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }
    }
}
