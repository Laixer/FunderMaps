using FunderMaps.Core.Components;
using FunderMaps.Core.Email;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    public static IServiceCollection AddFunderMapsCoreServices2(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Register core components in DI container.
        // NOTE: These services are rarely used and should therefore be
        //       registered as transient. They are re-instantiated on every
        //       resolve and disposed right after.
        services.AddTransient<IRandom, RandomGenerator>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        // services.AddTransient<IGeocoderParser, GeocoderParser>();
        // services.AddTransient<IGeocoderTranslation, GeocoderTranslation>();

        // Register application context in DI container
        // NOTE: The application context *must* be registered with the container
        //       in order for core services to be functional. This registration is
        //       merely a placeholder. The front framework should bootstrap the application
        //       context if possible.
        services.AddSingleton<IAppContextFactory, AppContextFactory>();
        services.AddScoped<FunderMaps.Core.AppContext>(serviceProvider => serviceProvider.GetRequiredService<IAppContextFactory>().Create());

        services.TryAddSingleton<IEmailService, MailgunService>();
        services.TryAddSingleton<IBlobStorageService, SpacesBlobStorageService>();

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

        AddFunderMapsCoreServices2(services);

        // The startup essential properties can be used to setup components.
        (Configuration, _) = services.BuildStartupProperties();

        // Register core components in DI container.
        // NOTE: These services are rarely used and should therefore be
        //       registered as transient. They are re-instantiated on every
        //       resolve and disposed right after.
        // services.AddTransient<IRandom, RandomGenerator>();
        // services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IGeocoderParser, GeocoderParser>();
        services.AddTransient<IGeocoderTranslation, GeocoderTranslation>();

        // Register application context in DI container
        // NOTE: The application context *must* be registered with the container
        //       in order for core services to be functional. This registration is
        //       merely a placeholder. The front framework should bootstrap the application
        //       context if possible.
        // services.AddSingleton<IAppContextFactory, AppContextFactory>();
        // services.AddScoped<FunderMaps.Core.AppContext>(serviceProvider => serviceProvider.GetRequiredService<IAppContextFactory>().Create());

        // Register core services in DI container.
        // NOTE: These services take time to initialize are used more often. Registering
        //       them as a singleton will keep the services alife for the entire lifetime
        //       of the application. Beware to add new services as singletons.
        services.Configure<MailgunOptions>(Configuration.GetSection(MailgunOptions.Section));
        // services.TryAddSingleton<IEmailService, MailgunService>();

        // Register core services in DI container.
        // NOTE: These services take time to initialize are used more often. Registering
        //       them as a singleton will keep the services alife for the entire lifetime
        //       of the application. Beware to add new services as singletons.
        services.Configure<BlobStorageOptions>(Configuration.GetSection(BlobStorageOptions.Section));
        // services.TryAddSingleton<IBlobStorageService, SpacesBlobStorageService>();

        // Register the incident core service.
        services.AddScoped<IIncidentService, IncidentService>();
        services.Configure<IncidentOptions>(Configuration.GetSection(IncidentOptions.Section));

        return services;
    }
}
