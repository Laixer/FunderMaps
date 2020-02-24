using FunderMaps.Cloud;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FunderMaps.Extensions
{
    /// <summary>
    /// Provides extension methods for services from application cloud interface.
    /// </summary>
    public static class ApplicationCloudServiceCollectionExtensions
    {
        // TODO: Remove the IConfiguration

        /// <summary>
        /// Adds the application cloud services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">See <see cref="IConfiguration"/>.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddApplicationCloudServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddTransient<IFileStorageService, AzureBlobStorageService>();

            services.Configure<FileStorageOptions>(options =>
            {
                if (options.StorageContainers == null)
                {
                    options.StorageContainers = new Dictionary<string, string>();
                }

                var rootKey = configuration.GetSection("FileStorageContainers");
                if (rootKey == null)
                {
                    return; // TODO: This can never be oke.
                }

                // FUTURE: This can drastically be improved.
                foreach (var item in rootKey.GetChildren())
                {
                    options.StorageContainers.Add(item.Key, item.Value);
                }
            });

            return services;
        }
    }
}
