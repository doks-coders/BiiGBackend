using BiiGBackend.Models.Entities.Orders;
using PayPal.Api;
using Serilog;

namespace BiiGBackend.ApplicationCore.Services
{


    public static class PayPalConfiguration
    {
        // Returns PayPal API context using credentials
        public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            // Configuration for the PayPal environment and credentials
            var config = new Dictionary<string, string>
        {
            { "mode", mode },  // Use "live" for live transactions
            { "clientId", clientId},
            { "clientSecret", clientSecret}
        };



            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken)
            {
                Config = config
            };
            return apiContext;


            /*
                        string apiBaseUrl = config["mode"] == "live"
                                ? "https://api-m.paypal.com"  // Live URL
                                : "https://api-m.sandbox.paypal.com";  // Sandbox URL

                        // Use the API URL in your OAuthTokenCredential
                        var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                        var apiContext = new APIContext(accessToken)
                        {
                            Config = new Dictionary<string, string> { { "baseUrl", apiBaseUrl } }
                        };
                        return apiContext;
                        */
        }
    }

    public class PaypalService : IPaypalService
    {
        private readonly APIContext _apiContext;
        public PaypalService(APIContext apiContext)
        {
            _apiContext = apiContext;
        }
        public Payment CreatePayment(string redirectUrl, double totalAmount, IEnumerable<OrderItem> orderItems, double shippingPrice)
        {
            // Define payment object
            var apiContext = _apiContext;

            Log.Information("Access Token Accessed");
            Log.Information(_apiContext.AccessToken);

            var item_list = new ItemList();
            item_list.items = orderItems.Select(u => new Item()
            {
                name = u.Product.ProductName,
                currency = "USD",
                price = (u.OrderedDisplayPrice).ToString(),
                quantity = u.Count.ToString(),
                sku = "sku"
            }).ToList();
            item_list.items.Add(new Item()
            {
                name = "Shipping",
                currency = "USD",
                price = shippingPrice.ToString(),
                quantity = "1",
                sku = "sku"
            });
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                transactions = new List<Transaction>
        {
            new Transaction
            {
                description = "Transaction description.",
                invoice_number = Guid.NewGuid().ToString(), // Unique identifier
                amount = new Amount
                {
                    currency = "USD",
                    total = totalAmount.ToString(), // Payment amount
                },
                item_list =item_list
            }
        },
                redirect_urls = new RedirectUrls
                {
                    cancel_url = redirectUrl + "?cancel=true",
                    return_url = redirectUrl
                }
            };

            // Create the payment
            return payment.Create(apiContext);
        }

    }

    public interface IPaypalService
    {
        Payment CreatePayment(string redirectUrl, double totalAmount, IEnumerable<OrderItem> orderItems, double shippingPrice);
    }
}
