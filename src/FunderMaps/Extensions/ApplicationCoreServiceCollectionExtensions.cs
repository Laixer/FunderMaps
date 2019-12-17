using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for services from application core.
    /// </summary>
    public static class ApplicationCoreServiceCollectionExtensions
    {
        // TODO: Remove the IConfiguration

        /// <summary>
        /// Adds the application core services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">See <see cref="IConfiguration"/>.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IGeoService, GeoService>();

            return services;
        }
    }
}
