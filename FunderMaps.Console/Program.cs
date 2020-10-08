using FunderMaps.Console.Services;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Console
{
    /// <summary>
    ///     Main entry class.
    /// </summary>
    public class Program
    {
        private static readonly Guid OrganizationId = new Guid("11cdc51f-3ba6-4e3c-b975-dfed4f054c31");
        private static readonly Guid BundleId = new Guid("06df6716-2af0-4359-b2e5-c8a207c99b82");

        // TODO Environment variables.
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static async Task Main(string[] args)
        {
            // TODO This is a WIP.
            if (args == null || args.Length == 0)
            {
                throw new InvalidOperationException("Missing config file arg");
            }

            var services = new ServiceCollection();

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
            services.AddScoped<BundleBuildingService>();
            services.AddScoped<BundleStorageService>();
            services.Configure<ConsoleOptions>(config => configuration.GetSection("ConsoleOptions").Bind(config));

            // Add logger.
            // TODO Look into this
            services.AddLogging();

            var provider = services.BuildServiceProvider();

            // TODO Remove this, debug
            var ss = provider.GetRequiredService<BundleStorageService>();
            var exportedFormats = await ss.GetExportedFormatsAsync(BundleId, 53);
            var currentVersion = await ss.GetCurrentExportedVersionAsync(OrganizationId, BundleId);

            var service = provider.GetRequiredService<BundleBuildingService>();
            var formats = new List<GeometryExportFormat> { GeometryExportFormat.Mvt };
            await service.BuildAllBundlesAsync(formats, force: true);
        }
    }
}
