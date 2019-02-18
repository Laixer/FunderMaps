using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Data.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<FunderMapsUser> userManager, RoleManager<FunderMapsRole> roleManager)
        {
            // Check if seeding is done already
            if (userManager.Users.Any()) { return; }

            await roleManager.CreateAsync(new FunderMapsRole(Constants.AdministratorRole));

            var adminUser = new FunderMapsUser("admin@contoso.com")
            {
                JobTitle = "Administrator",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser);

            var defaultUser = new FunderMapsUser("user@contoso.com")
            {
                EmailConfirmed = true
            };
            await userManager.CreateAsync(defaultUser);

            await userManager.AddToRoleAsync(adminUser, Constants.AdministratorRole);
        }
    }
}
