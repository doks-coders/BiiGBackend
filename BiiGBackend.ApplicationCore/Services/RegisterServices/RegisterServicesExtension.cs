using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Infrastructure.Repositories;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BiiGBackend.ApplicationCore.Services.RegisterServices
{
	public static class RegisterServicesExtension
	{
		public static IServiceCollection RegisterServices(this IServiceCollection services)
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
			return services;
		}
	}


}
