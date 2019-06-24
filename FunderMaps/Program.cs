using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Data.Seed;
using FunderMaps.Models.Identity;

namespace FunderMaps
{
    /// <summary>
    /// Application entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static async Task Main(string[] args)
        {
            var host = BuildWebHost(args);

            DatabaseInitialization(host);

            await host.RunAsync();
        }

        /// <summary>
        /// Build a webhost and run the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();

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
                var config = services.GetRequiredService<IConfiguration>();

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
                    IdentitySeed.SeedAsync(env, userManager, roleManager, config).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }
        }
    }
}
