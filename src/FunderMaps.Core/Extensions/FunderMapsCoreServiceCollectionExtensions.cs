using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Managers;
using FunderMaps.Core.Services;
using FunderMaps.Core.UseCases;
using FunderMaps.Webservice.Abstractions.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the core services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsCoreServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register core objects in DI container.
            services.AddSingleton<IRandom, RandomGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            // TODO: AddScoped -> Transient?
            // Register core service fillers in DI container.
            services.AddScoped<IEmailService, NullEmailService>();
            services.AddScoped<IBlobStorageService, NullBlobStorageService>();
            services.AddScoped<INotificationService, NullNotificationService>();

            // Register core use cases in DI container.
            services.AddScoped<GeocoderUseCase>();
            services.AddScoped<IncidentUseCase>();
            services.AddScoped<InquiryUseCase>();
            services.AddScoped<ProjectUseCase>();
            services.AddScoped<RecoveryUseCase>();

            // Register core managers in DI container.
            services.AddScoped<UserManager>();
            services.AddScoped<OrganizationManager>();

            // Register core services in DI container.
            services.AddTransient<IProductService, ProductService>();

            return services;
        }
    }
}
