using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.Requests;
using BiiGBackend.StaticDefinitions.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BiiGBackend.ApplicationCore.Utitlity
{
    public class AppUserSeed
    {
        public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<AppRole> roleManager, ILogger logger, IAuthService authService)
        {
            if (!await userManager.Users.Where(u => u.Email == "admin@godanddevil.death").AnyAsync())
            {
                logger.LogInformation("Seed Started Successfully");

                List<AppRole> appRoles = new List<AppRole>() {
                new AppRole(){Name=RoleConstants.Customer},
                new AppRole(){Name=RoleConstants.Admin},
                new AppRole(){Name=RoleConstants.Technician},
                };
                foreach (AppRole appRole in appRoles)
                {
                    var res = await roleManager.CreateAsync(appRole);
                    logger.LogError(JsonSerializer.Serialize(res.Succeeded));
                }

                await authService.Register(new RegisterUserRequestComplete() { AccountType = RoleConstants.Admin, Email = "admin@godanddevil.death", Password = "John1135@Bible", Verify = "John1135@Bible" }, RoleConstants.Admin);
                logger.LogInformation("Seed Ran Successfully");

            }
        }
    }
}
