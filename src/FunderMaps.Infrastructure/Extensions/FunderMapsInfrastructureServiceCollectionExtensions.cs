using FunderMaps.Core.Interfaces;
using FunderMaps.Infrastructure.Email;
using FunderMaps.Infrastructure.Notification;
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
        ///     Use this method to add production services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public static void ConfigureProductionServices(IServiceCollection services)
        {
            // Remove all existing email services and inject local email service.
            services.RemoveAll<IEmailService>();
            services.Configure<EmailOptions>(Configuration.GetSection(EmailOptions.Section));
            services.AddSingleton<IEmailService, EmailService>();

            // Remove all existing file storage services and inject local file stoage service.
            services.RemoveAll<IBlobStorageService>();
            // FUTURE: Is there another way to configure this service?
            services.Configure<FileStorageOptions>(options =>
            {
                var rootKey = Configuration.GetSection(FileStorageOptions.Section);

                // FUTURE: This can be improved.
                foreach (var item in rootKey?.GetChildren())
                {
                    options.StorageContainers.Add(item.Key, item.Value);
                }
            });
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

            // Remove all existing notification services and inject local email service.
            services.RemoveAll<INotificationService>();
            services.AddTransient<INotificationService, NotificationHubService>();
        }

        /// <summary>
        ///     Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            // Remove all existing notification services and inject local email service.
            services.RemoveAll<INotificationService>();
            services.AddTransient<INotificationService, NotificationHubService>();
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

            if (HostEnvironment.IsProduction())
            {
                ConfigureProductionServices(services);
            }
            else
            {
                ConfigureServices(services);
            }
            return services;
        }
    }
}
