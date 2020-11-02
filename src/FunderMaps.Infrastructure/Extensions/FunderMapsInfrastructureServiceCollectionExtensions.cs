using FunderMaps.Core.Interfaces;
using FunderMaps.Infrastructure.Email;
using FunderMaps.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services.RemoveAll<IEmailService>();
            services.Configure<EmailOptions>(Configuration.GetSection(EmailOptions.Section));
            services.AddSingleton<IEmailService, EmailService>();

            // Remove all existing file storage services and inject local file stoage service.
            services.RemoveAll<IBlobStorageService>();
            services.Configure<BlobStorageOptions>(Configuration.GetSection("BlobStorage"));
            services.AddSingleton<IBlobStorageService, SpacesBlobStorageService>();
        }

        /// <summary>
        ///     Adds the infrastructure services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsInfrastructureServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            using var serviceProviderScope = services.BuildServiceProvider().CreateScope();
            Configuration = serviceProviderScope.ServiceProvider.GetRequiredService<IConfiguration>();
            HostEnvironment = serviceProviderScope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            if (!HostEnvironment.IsDevelopment())
            {
                ConfigureExternalServices(services);
            }

            return services;
        }
    }
}
