using FunderMaps.Console.BundleServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Console
{
    /// <summary>
    ///     Main entry class.
    /// </summary>
    public class Program
    {
        //private static readonly Guid OrganizationId = new Guid("11cdc51f-3ba6-4e3c-b975-dfed4f054c31");
        //private static readonly Guid BundleId = new Guid("06df6716-2af0-4359-b2e5-c8a207c99b82");

        // TODO Environment variables.
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    Configure(services, args);
                })
                .Build()
                .RunAsync();

        // TODO Clean up.
        /// <summary>
        ///     Configure services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="args"></param>
        private static void Configure(IServiceCollection services, string[] args)
        {
            // TODO This is a WIP.
            if (args == null || args.Length == 0)
            {
                throw new InvalidOperationException("Missing config file arg");
            }

            // TODO Correctly configure everything.
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(args[0], optional: false)
                .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Add FunderMaps services.
            services.AddFunderMapsCoreServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");
            services.AddSpacesBlobStorageServices(configuration, "BlobStorage");

            // Add console services.
            services.AddSingleton<BundleBuildingService>();
            services.AddSingleton<BundleStorageService>();
            services.Configure<ConsoleOptions>(config => configuration.GetSection("ConsoleOptions").Bind(config));

            // Add logger.
            // TODO Look into this - does hostbuilder do this for us?
            services.AddLogging();
        }
    }
}
