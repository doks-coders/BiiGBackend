using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Models.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BiiGBackend.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<ApplicationUser>(e =>
            {
                e.Password.RequireUppercase = false;
                e.Password.RequireLowercase = false;
                e.Password.RequiredLength = 6;
                e.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(u =>
            {
                u.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JwtOptions:Key").Value)),
                };
            });

            return services;
        }
    }
}
