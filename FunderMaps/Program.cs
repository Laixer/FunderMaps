using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Data.Seed;
using FunderMaps.Models.Identity;

namespace FunderMaps
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = BuildWebHost(args).Build();

            DatabaseInitialization(host);

            await host.RunAsync();
        }

        public static IWebHostBuilder BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void DatabaseInitialization(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<FunderMapsDbContext>();
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                    FunderMapsSeed.SeedAsync(dbContext).Wait();

                    var userManager = services.GetRequiredService<UserManager<FunderMapsUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<FunderMapsRole>>();
                    IdentitySeed.SeedAsync(userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
