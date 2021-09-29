using FunderMaps.Core.Components;
using FunderMaps.Core.Email;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.MapBundle;
using FunderMaps.Core.MapBundle.Jobs;
using FunderMaps.Core.Model;
using FunderMaps.Core.Model.Jobs;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Notification.Jobs;
using FunderMaps.Core.Services;
using FunderMaps.Core.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreServiceCollectionExtensions
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
        ///     Adds batch job to the task component.
        /// </summary>
        public static IServiceCollection AddBatchJob<TBatchJob>(this IServiceCollection services)
        {
            services.AddTransient(typeof(TBatchJob));
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(TBatchJob)));

            return services;
        }

        /// <summary>
        ///     Adds the core threading service to the container.
        /// </summary>
        private static IServiceCollection AddCoreThreading(this IServiceCollection services)
        {
            services.AddScoped<BackgroundTaskScopedDispatcher>();
            services.AddSingleton<DispatchManager>();
            services.AddTransient<BackgroundTaskDispatcher>();
            services.Configure<BackgroundWorkOptions>(Configuration.GetSection(BackgroundWorkOptions.Section));

            return services;
        }

        /// <summary>
        ///     Adds incident reporting service.
        /// </summary>
        private static IServiceCollection AddIncident(this IServiceCollection services)
        {
            services.AddBatchJob<EmailJob>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.Configure<IncidentOptions>(Configuration.GetSection(IncidentOptions.Section));

            return services;
        }

        /// <summary>
        ///     Adds map bundle service.
        /// </summary>
        private static IServiceCollection AddMapBundle(this IServiceCollection services)
        {
            services.AddBatchJob<ExportJob>();
            services.AddScoped<IBundleService, BundleHub>();

            return services;
        }

        /// <summary>
        ///     Adds model service.
        /// </summary>
        private static IServiceCollection AddModel(this IServiceCollection services)
        {
            services.AddBatchJob<RefreshJob>();
            services.AddScoped<IModelService, ModelService>();

            return services;
        }

        /// <summary>
        ///     Adds the <see cref="AppContext"/> to the container.
        /// </summary>
        private static IServiceCollection AddAppContext(this IServiceCollection services)
        {
            services.AddSingleton<IAppContextFactory, AppContextFactory>();
            services.AddScoped<FunderMaps.Core.AppContext>(serviceProvider => serviceProvider.GetRequiredService<IAppContextFactory>().Create());

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
            if (services is not IServiceCollection)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // The startup essential properties can be used to setup components.
            (Configuration, HostEnvironment) = services.BuildStartupProperties();

            // Register core components in DI container.
            // NOTE: These services are rarely used and should therefore be
            //       registered as transient. They are re-instantiated on every
            //       resolve and disposed right after.
            services.AddTransient<IRandom, RandomGenerator>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITemplateParser, TemplateParser>();
            services.AddTransient<IGeocoderParser, GeocoderParser>();
            services.AddTransient<IGeocoderTranslation, GeocoderTranslation>();

            // Register application context in DI container
            // NOTE: The application context *must* be registered with the container
            //       in order for core services to be functional. This registration is
            //       merely a placeholder. The front framework should bootstrap the application
            //       context if possible.
            services.AddAppContext();

            // Register core services in DI container.
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INotifyService, NotificationHub>();

            // Register core services in DI container.
            // NOTE: These services take time to initialize are used more often. Registering
            //       them as a singleton will keep the services alife for the entire lifetime
            //       of the application. Beware to add new services as singletons.
            services.TryAddSingleton<IEmailService, NullEmailService>();
            services.TryAddTransient<IMapService, NullMapService>();
            services.TryAddTransient<IBlobStorageService, NullBlobStorageService>();

            // The application core (as well as many other components) depends upon the ability to cache
            // objects to memory. The memory cache may have already been registered with the container
            // by some other package, however we cannot expect this to be.
            services.AddMemoryCache();

            // Register the incident core service.
            services.AddIncident();

            // Register the map bundle service.
            services.AddMapBundle();

            // Register the model service.
            services.AddModel();

            // The application core (as well as many other components) depends upon the ability to dispatch
            // tasks to the background.
            services.AddCoreThreading();

            return services;
        }
    }
}
