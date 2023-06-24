using FunderMaps.Core.Components;
using FunderMaps.Core.Email;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsCoreServiceCollectionExtensions
{
    /// <summary>
    ///     Configuration.
    /// </summary>
    public static IConfiguration Configuration { get; set; } = default!;

    /// <summary>
    ///     Host environment.
    /// </summary>
    public static IHostEnvironment HostEnvironment { get; set; } = default!;

    /// <summary>
    ///     Adds incident reporting service.
    /// </summary>
    private static IServiceCollection AddIncident(this IServiceCollection services)
    {
        services.AddScoped<IIncidentService, IncidentService>();
        services.Configure<IncidentOptions>(Configuration.GetSection(IncidentOptions.Section));

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
        if (services is null)
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
        services.AddTransient<IGeocoderParser, GeocoderParser>();
        services.AddTransient<IGeocoderTranslation, GeocoderTranslation>();

        // Register application context in DI container
        // NOTE: The application context *must* be registered with the container
        //       in order for core services to be functional. This registration is
        //       merely a placeholder. The front framework should bootstrap the application
        //       context if possible.
        services.AddAppContext();

        // Register core services in DI container.
        // NOTE: These services take time to initialize are used more often. Registering
        //       them as a singleton will keep the services alife for the entire lifetime
        //       of the application. Beware to add new services as singletons.
        services.TryAddSingleton<IEmailService, NullEmailService>();
        services.TryAddTransient<IBlobStorageService, NullBlobStorageService>();

        // The application core (as well as many other components) depends upon the ability to cache
        // objects to memory. The memory cache may have already been registered with the container
        // by some other package, however we cannot expect this to be.
        services.AddMemoryCache();

        // Register the incident core service.
        services.AddIncident();

        return services;
    }
}
