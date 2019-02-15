using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FunderMaps.Models.Identity;

namespace FunderMaps.Data.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<FunderMapsUser> userManager, RoleManager<FunderMapsRole> roleManager)
        {
            // Check if seeding is done already
            if (userManager.Users.Any()) { return; }

            await roleManager.CreateAsync(new FunderMapsRole("Administrator"));

            var adminUser = new FunderMapsUser("admin@contoso.com")
            {
                JobTitle = "Administrator",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser);

            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}
