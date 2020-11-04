using FunderMaps.Console.BackgroundTasks;
using FunderMaps.Console.BundleServices;
using FunderMaps.Console.Dev;
using FunderMaps.Core.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Console
{
    /// <summary>
    ///     Main entry class.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, config) =>
                {
                    // TODO Do correctly
                    config.SetMinimumLevel(LogLevel.Trace);
                    config.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    Configure(services, args);
                })
                .Build()
                .RunAsync();

        /// <summary>
        ///     Configure services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="args"></param>
        private static void Configure(IServiceCollection services, string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new InvalidOperationException("Missing config file arg");
            }

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(args[0], optional: false)
                .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Add FunderMaps services.
            services.AddFunderMapsCoreServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");
            services.AddSpacesBlobStorageServices(configuration, "BlobStorage");

            // TODO Remove
            services.AddHostedService<DevEnqueueMachine>();

            // Add all types of background tasks as an enumerable.
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(BundleBuildingTask)),
                ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(DummyTask)),
                ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(CommandTask)),
            });

            // Add console services.
            services.Configure<BackgroundWorkOptions>(config => configuration.GetSection("BackgroundWorkOptions").Bind(config));
            services.Configure<BundleBuildingOptions>(config => configuration.GetSection("BundleBuildingOptions").Bind(config));

            // Add console services.
            services.AddScoped<BundleStorageService>();
        }
    }
}
