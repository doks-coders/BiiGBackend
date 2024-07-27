using BiiGBackend.ApplicationCore.Services.RegisterServices;
using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Web.Extensions;
using BiiGBackend.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace BiiGBackend.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.ConfigureServices(builder.Configuration,builder.Environment);
			builder.Services.ConfigureIdentity(builder.Configuration);
			builder.Services.RegisterServices();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder => builder
						.WithOrigins("https://localhost:4200")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddHttpContextAccessor();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseDefaultFiles();
			app.UseStaticFiles();
			StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

			app.UseCors(u => u.AllowAnyHeader().AllowAnyMethod()
				.AllowCredentials()
				.WithOrigins("https://localhost:4200", "http://localhost:4200"));



			app.UseMiddleware<ExceptionMiddleware>();

			app.MapFallbackToController("Index", "Fallback");
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();


			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{

					var db = services.GetRequiredService<ApplicationDbContext>();
					//await db.Database.EnsureDeletedAsync();
					await db.Database.MigrateAsync();
					var logger = services.GetService<ILogger<Program>>();
					logger.LogInformation("Migration Successfull");

				}
				catch (Exception ex)
				{
					var logger = services.GetService<ILogger<Program>>();
					logger.LogError(ex, "An Error Occurred during Migration");
				}

			}

			app.Run();
		}
	}
}


// "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BiiGBackend;Trusted_Connection=True;"