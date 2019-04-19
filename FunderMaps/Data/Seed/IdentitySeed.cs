using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Data.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(
            IHostingEnvironment env,
            UserManager<FunderMapsUser> userManager,
            RoleManager<FunderMapsRole> roleManager,
            IConfiguration configuration)
        {
            // Check if seeding is done already.
            if (userManager.Users.Any()) { return; }

            await roleManager.CreateAsync(new FunderMapsRole(Constants.AdministratorRole));

            var adminUser = new FunderMapsUser("admin@contoso.com")
            {
                JobTitle = "Administrator",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, configuration["AdminUserPassword"]);

            await userManager.AddToRoleAsync(adminUser, Constants.AdministratorRole);

            if (env.IsDevelopment() || env.IsStaging())
            {
                await SeedTestingDataAsync(userManager);
            }
        }

        private static async Task SeedTestingDataAsync(UserManager<FunderMapsUser> userManager)
        {
            var defaultUser = new FunderMapsUser("user@contoso.com")
            {
                EmailConfirmed = true
            };
            await userManager.CreateAsync(defaultUser);
        }
    }
}
