using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Infrastructure.BatchClient;
using FunderMaps.Infrastructure.Email;
using FunderMaps.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    // FUTURE: Introduce a Startup class much like FunderMaps.WebApi.Startup

    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsInfrastructureServiceCollectionExtensions
    {
        /// <summary>
        ///     Configuration.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        ///     Host environment.
        /// </summary>
        public static IHostEnvironment HostEnvironment { get; set; }

        /// <summary>
        ///     Use this method to add external services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        private static void ConfigureExternalServices(IServiceCollection services)
        {
            // Remove all existing email services and inject local email service.
            services.AddOrReplace<IEmailService, SmtpService>(ServiceLifetime.Singleton);
            services.Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.Section));

            // Remove all existing file storage services and inject local file stoage service.
            services.AddOrReplace<IBlobStorageService, SpacesBlobStorageService>(ServiceLifetime.Singleton);
            services.Configure<BlobStorageOptions>(Configuration.GetSection("BlobStorage"));

            // Remove all existing batch services and inject local batch service.
            services.AddSingleton<ChannelFactory>();
            services.AddOrReplace<IBatchService, BatchProxy>(ServiceLifetime.Singleton);
            services.Configure<BatchOptions>(Configuration.GetSection("Batch"));
        }

        /// <summary>
        ///     Adds the infrastructure services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsInfrastructureServices(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // The startup essential properties can be used to setup components.
            (Configuration, HostEnvironment) = services.BuildStartupProperties();

            if (!HostEnvironment.IsDevelopment() || Configuration.GetValue<bool>("UseExternalServices", false))
            {
                ConfigureExternalServices(services);
            }

            return services;
        }

        /// <summary>
        ///     Explicitly add <see cref="IBlobStorageService"/> to the services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Configuration collection.</param>
        /// <param name="configurationSection">The name of the configuration section.</param>
        /// <returns>Chained <paramref name="services"/>.</returns>
        public static IServiceCollection AddSpacesBlobStorageServices(this IServiceCollection services, IConfiguration configuration, string configurationSection)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<IBlobStorageService, SpacesBlobStorageService>();
            services.Configure<BlobStorageOptions>(options => configuration.GetSection(configurationSection).Bind(options));

            return services;
        }
    }
}
