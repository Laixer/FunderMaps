﻿using FunderMaps.Core.Components;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Services;
using FunderMaps.Core.Threading;
using FunderMaps.Core.UseCases;
using FunderMaps.Webservice.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the core threading service to the container.
        /// </summary>
        private static IServiceCollection AddCoreThreading(this IServiceCollection services)
        {
            services.AddSingleton<DispatchManager>();
            // services.Configure<BackgroundWorkOptions>(options => configuration.GetSection("BackgroundWorkOptions").Bind(options));
            services.Configure<BackgroundWorkOptions>(options =>
            {
                options.MaxQueueSize = 8192;
                options.MaxWorkers = 2;
                options.TimeoutDelay = TimeSpan.FromMinutes(30);
            });

            return services;
        }

        /// <summary>
        ///     Adds the core services to the container.
        /// </summary>
        /// <remarks>
        ///     Read the instructions before adding a service.
        ///     <para>
        ///         Add service components with their correct lifetime cycle. An invalid lifetime can
        ///         block the dependency graph resulting in an underperforming application.
        ///     </para>
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsCoreServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register core components in DI container.
            // NOTE: These services are rarely used and should therefore be
            //       registered as transient. They are re-instantiated on every
            //       resolve and disposed right after.
            services.AddTransient<IRandom, RandomGenerator>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            // Register application context in DI container
            // NOTE: The application context *must* be registered with the container
            //       in order for core services to be functional. This registration is
            //       merely a placeholder. The front framework should setup the application
            //       context.
            services.TryAddScoped<FunderMaps.Core.AppContext>();

            // Register core use cases in DI container.
            services.AddScoped<ProjectUseCase>();
            services.AddScoped<RecoveryUseCase>();

            // Register core services in DI container.
            services.AddScoped<IProductService, ProductService>();

            // Register core services in DI container.
            // NOTE: These services take time to initialize are used more often. Registering
            //       them as a singleton will keep the services alife for the entire lifetime
            //       of the application. Beware to add new services as singletons.
            services.TryAddSingleton<IEmailService, NullEmailService>();
            services.TryAddSingleton<IBlobStorageService, NullBlobStorageService>();
            services.TryAddSingleton<INotifyService, NotificationHub>();

            // Register notification handlers in DI container.
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient<INotifyHandler, EmailHandler>(),
            });

            // The application core (as well as many other components) depends upon the ability to cache
            // objects to memory. The memory cache may have already been registered with the container
            // by some other package, however we cannot expect this to be.
            services.AddMemoryCache();

            // The application core (as well as many other components) depends upon the ability to dispatch
            // tasks to the background.
            services.AddCoreThreading();

            return services;
        }
    }
}
