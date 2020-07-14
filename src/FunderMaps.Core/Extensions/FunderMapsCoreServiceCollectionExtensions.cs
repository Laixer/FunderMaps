using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using FunderMaps.Core.UseCases;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the core services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsCoreServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IGeocoderService, GeoService>();

            services.AddScoped<IncidentUseCase>();
            services.AddScoped<InquiryUseCase>();
            services.AddScoped<ProjectUseCase>();
            services.AddScoped<RecoveryUseCase>();

            return services;
        }
    }
}
