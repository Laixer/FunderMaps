using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering services from core.
    /// </summary>
    public static class CoreServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the core services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">See <see cref="IConfiguration"/>.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFileStorageService, AzureBlobStorageService>();

            services.Configure<FileStorageOptions>(options =>
            {
                if (options.StorageContainers == null)
                {
                    options.StorageContainers = new Dictionary<string, string>();
                }

                var rootKey = configuration.GetSection("FileStorageContainers");
                if (rootKey == null) { return; }

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
