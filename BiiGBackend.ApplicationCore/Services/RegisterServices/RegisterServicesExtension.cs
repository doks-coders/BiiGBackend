using BiiGBackend.ApplicationCore.Services.GeoPlaces;
using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using ChatUpdater.ApplicationCore.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BiiGBackend.ApplicationCore.Services.RegisterServices
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStaticItemsService, StaticItemsService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IStripePayment, StripePayment>();
            services.AddScoped<IPaystackService, PaystackService>();
            services.AddScoped<IPaypalService, PaypalService>();
            services.AddScoped<IEmailSender, EmailSender>();
			services.AddScoped<IWishListService, WishListService>();
			services.AddSingleton<GeoPlacesService>();
            services.AddScoped(u => PayPalConfiguration.GetAPIContext(config.GetSection("Paypal:ClientId").Value, config.GetSection("Paypal:ClientSecret").Value, config.GetSection("Paypal:Mode").Value));
            return services;
        }
    }
}
