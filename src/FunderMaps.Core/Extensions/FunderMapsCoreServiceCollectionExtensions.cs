using FunderMaps.Core.Authentication;
using FunderMaps.Core.Authorization;
using FunderMaps.Core.Components;
using FunderMaps.Core.DataProtection;
using FunderMaps.Core.ExternalServices;
using FunderMaps.Core.HealthChecks;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Options;
using FunderMaps.Core.Services;
using FunderMaps.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsCoreServiceCollectionExtensions
{
    private static readonly string[] externalTags = ["extern"];
    private static readonly string[] localTags = ["local"];

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
        services.AddTransient<PasswordHasher>();
        services.AddTransient<GeocoderTranslation>();
        services.AddTransient<ModelService>();
        services.AddScoped<IncidentService>(); // TODO: Should be transient?
        services.AddScoped<SignInService>(); // TODO: Should be transient?

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
        services.AddSingleton<OpenAIService>();
        services.AddSingleton<FunderMapsClient>();

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

        return services;
    }

    public static IServiceCollection AddFunderMapsAuthServices(this IServiceCollection services)
    {
        services.AddTransient<ISecurityTokenProvider, JwtBearerTokenProvider>();

        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        // TODO: Requesting the repository directly is not the best way to do this. This cannot be replaced later on.
        var keystoreRepository = serviceProvider.GetRequiredService<FunderMaps.Core.Interfaces.Repositories.IKeystoreRepository>();

        services.Configure<KeyManagementOptions>(options =>
        {
            options.XmlRepository = new KeystoreXmlRepository(keystoreRepository);
        });

        services.AddDataProtection().SetApplicationName(configuration["DataProtection:ApplicationName"] ?? throw new InvalidOperationException("Application name not set"));

        services.AddAuthentication("FunderMapsHybridAuth")
            .AddFunderMapsScheme()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new JwtTokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer not found in configuration."),
                    ValidAudience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience not found in configuration."),
                    IssuerSigningKey = configuration.GetJwtSigningKey(),
                    Valid = configuration.GetJwtTokenExpirationInMinutes(),
                };
            })
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.Cookie.Name = configuration["Authentication:Cookie:Name"];
                options.Cookie.Domain = configuration["Authentication:Cookie:Domain"];
            })
            .AddScheme<AuthKeyAuthenticationOptions, AuthKeyAuthenticationHandler>(AuthKeyAuthenticationOptions.DefaultScheme, options =>
            {
                //
            });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddFunderMapsPolicy();
        });

        return services;
    }

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
