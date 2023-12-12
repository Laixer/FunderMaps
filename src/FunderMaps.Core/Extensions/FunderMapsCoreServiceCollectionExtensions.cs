using FunderMaps.Core.Components;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.Core.ExternalServices.Mailgun;
using FunderMaps.Core.ExternalServices.Mapbox;
using FunderMaps.Core.ExternalServices.OpenAI;
using FunderMaps.Core.ExternalServices.S3Storage;
using FunderMaps.Core.ExternalServices.Tippecanoe;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsCoreServiceCollectionExtensions
{
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
        // Register core components in DI container.
        // NOTE: These services are rarely used and should therefore be
        //       registered as transient. They are re-instantiated on every
        //       resolve and disposed right after.
        services.AddTransient<IRandom, RandomGenerator>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<GeocoderTranslation>();

        // Register application context in DI container
        // NOTE: The application context *must* be registered with the container
        //       in order for core services to be functional. This registration is
        //       merely a placeholder. The front framework should bootstrap the application
        //       context if possible.
        services.AddSingleton<IAppContextFactory, AppContextFactory>();
        services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IAppContextFactory>().Create());

        // Register external services in DI container.
        services.AddSingleton<IEmailService, MailgunService>();
        services.AddSingleton<IBlobStorageService, S3StorageService>();
        services.AddSingleton<ITilesetGeneratorService, TippecanoeService>();
        services.AddSingleton<IMapboxService, MapboxService>();
        services.AddSingleton<IGDALService, GeospatialAbstractionService>();
        services.AddTransient<ModelService>();
        services.AddSingleton<OpenAIService>();
        services.AddSingleton<FunderMapsClient>();

        // Register core services in DI container.
        services.AddScoped<IIncidentService, IncidentService>();

        return services;
    }
}
