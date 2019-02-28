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
                .UseApplicationInsights()
                .UseStartup<Startup>();

        /// <summary>
        /// Initialize the database by running the migrations and seeding the database.
        /// </summary>
        private static void DatabaseInitialization(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var env = services.GetRequiredService<IHostingEnvironment>();

                try
                {
                    var dbContext = services.GetRequiredService<FunderMapsDbContext>();

                    logger.LogInformation("Run database migrations.");
                    dbContext.Database.Migrate();

                    logger.LogInformation("Run application database seeder.");
                    FunderMapsSeed.SeedAsync(env, dbContext).Wait();

                    logger.LogInformation("Run identity database seeder.");
                    var userManager = services.GetRequiredService<UserManager<FunderMapsUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<FunderMapsRole>>();
                    IdentitySeed.SeedAsync(env, userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }
        }
    }
}
