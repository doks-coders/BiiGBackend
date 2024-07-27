
using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Models.SharedModels;
using Indian.Utility;
using Microsoft.EntityFrameworkCore;

namespace BiiGBackend.Web.Extensions
{
	public static class ApplicationExtensions
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config, IHostEnvironment environment)
		{

			services.Configure<JwtOptions>(config.GetSection("JwtOptions"));
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
			services.Configure<StripeSettings>(config.GetSection("Stripe"));
			//services.AddDbContext<ApplicationDbContext>(u => u.UseSqlServer(config.GetConnectionString("DefaultConnection")));
			var connString = "";
			if (environment.IsDevelopment())
			{
				connString = config.GetConnectionString("DefaultConnection");
			}
			else
			{

				// Use connection string provided at runtime by FlyIO.
				var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

				// Parse connection URL to connection string for Npgsql
				connUrl = connUrl.Replace("postgres://", string.Empty);
				var pgUserPass = connUrl.Split("@")[0];
				var pgHostPortDb = connUrl.Split("@")[1];
				var pgHostPort = pgHostPortDb.Split("/")[0];
				var pgDb = pgHostPortDb.Split("/")[1];
				var pgUser = pgUserPass.Split(":")[0];
				var pgPass = pgUserPass.Split(":")[1];
				var pgHost = pgHostPort.Split(":")[0];
				var pgPort = pgHostPort.Split(":")[1];
				var updatedHost = pgHost.Replace("flycast", "internal");

				connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

			}

			services.AddDbContext<ApplicationDbContext>(opt =>
			{
				opt.UseNpgsql(connString);
			});
			return services;
		}
	}
}
