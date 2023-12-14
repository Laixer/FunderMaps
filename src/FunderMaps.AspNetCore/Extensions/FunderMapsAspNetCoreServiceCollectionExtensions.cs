using FunderMaps.AspNetCore.HealthChecks;
using FunderMaps.AspNetCore.Middleware;
using FunderMaps.Core.Options;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsAspNetCoreServiceCollectionExtensions
{
    private static readonly string[] externalTags = ["extern"];
    private static readonly string[] localTags = ["local"];

    public static IServiceCollection AddFunderMapsAspNetCoreServices(this IServiceCollection services)
    {
        services.AddFunderMapsCoreServices();
        services.AddFunderMapsDataServices();

        // NOTE: Register the HttpContextAccessor service to the container.
        //       The HttpContextAccessor exposes a singleton holding the
        //       HttpContext within a scoped resolver, or null outside the scope.
        //       Some components require the HttpContext and its features when the
        //       related service is being resolved within the scope.
        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddCheck<MapboxHealthCheck>("mapbox_health_check", tags: externalTags)
            .AddCheck<RepositoryHealthCheck>("data_health_check", tags: externalTags)
            .AddCheck<EmailHealthCheck>("email_health_check", tags: externalTags)
            .AddCheck<BlobStorageHealthCheck>("blob_storage_health_check", tags: externalTags)
            .AddCheck<IOHealthCheck>("io_health_check", tags: localTags)
            .AddCheck<TippecanoeHealthCheck>("tileset_generator_health_check", tags: localTags);

        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        // Configure services with configuration.
        // Any application depending on ASP.NET Core should have an IConfiguration service registered.
        services.Configure<MailgunOptions>(configuration.GetSection(MailgunOptions.Section));
        services.Configure<MapboxOptions>(configuration.GetSection(MapboxOptions.Section));
        services.Configure<S3StorageOptions>(configuration.GetSection(S3StorageOptions.Section));
        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.Section));
        services.Configure<IncidentOptions>(configuration.GetSection(IncidentOptions.Section));
        services.Configure<FunderMapsOptions>(configuration.GetSection(FunderMapsOptions.Section));

        var connectionString = configuration.GetConnectionString("FunderMapsConnection");
        services.Configure<DbProviderOptions>(options =>
        {
            options.ConnectionString = connectionString;
            options.ApplicationName = FunderMaps.AspNetCore.Constants.ApplicationName;
        });

        return services;
    }

    // TODO: Should be obsolete.
    // public static IServiceCollection AddFunderMapsAspNetCoreControllers(this IServiceCollection services)
    // {
    //     // FUTURE: Only load specific parts.
    //     // NOTE: This will register all controllers in the FunderMaps.AspNetCore
    //     //       assemly regardless of authentication and authorization.
    //     services.AddControllers(options => options.Filters.Add(typeof(FunderMapsCoreExceptionFilter))).AddFunderMapsAssembly();

    //     return services;
    // }

    public static IServiceCollection AddCorsAllowAny(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        return services;
    }
}
