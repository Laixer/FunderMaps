using FunderMaps.Core.Interfaces;
using FunderMaps.Infrastructure.Email;
using FunderMaps.Infrastructure.Notification;
using FunderMaps.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsInfrastructureServiceCollectionExtensions
    {
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
            var configuration = serviceProviderScope.ServiceProvider.GetRequiredService<IConfiguration>();

            // Remove all existing file storage services and inject local file stoage service.
            //services.RemoveAll<IFileStorageService>();
            //services.AddScoped<IFileStorageService, AzureBlobStorageService>();

            // Remove all existing email services and inject local email service.
            services.RemoveAll<IEmailService>();
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Section));
            services.AddTransient<IEmailService, EmailService>();

            // Remove all existing notification services and inject local email service.
            services.RemoveAll<INotificationService>();
            services.AddTransient<INotificationService, NotificationHubService>();

            // FUTURE: Bind
            //services.Configure<FileStorageOptions>(options =>
            //{
            //    if (options.StorageContainers == null)
            //    {
            //        options.StorageContainers = new Dictionary<string, string>();
            //    }

            //    var rootKey = configuration.GetSection("FileStorageContainers");
            //    if (rootKey == null)
            //    {
            //        return; // TODO: This can never be oke.
            //    }

            //    // FUTURE: This can drastically be improved.
            //    foreach (var item in rootKey.GetChildren())
            //    {
            //        options.StorageContainers.Add(item.Key, item.Value);
            //    }
            //});

            return services;
        }
    }
}
