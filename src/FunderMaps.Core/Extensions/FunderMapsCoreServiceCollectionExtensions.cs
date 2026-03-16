using FunderMaps.Core.Authentication;
using FunderMaps.Core.Authorization;
using FunderMaps.Core.DataProtection;
using FunderMaps.Core.ExternalServices;
using FunderMaps.Core.HealthChecks;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Options;
using FunderMaps.Core.Services;
using FunderMaps.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFunderMapsCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<GeocoderTranslation>();
        services.AddTransient<ModelService>();
        services.AddScoped<FunderMaps.Core.AppContext>();

        // Register external services in DI container.
        services.AddSingleton<IEmailService, MailgunService>();
        services.AddSingleton<IBlobStorageService, S3StorageService>();
        services.AddSingleton<IMapboxService, MapboxService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddCheck<BlobStorageHealthCheck>("blob_storage_health_check", tags: externalTags);

        services.Configure<MailgunOptions>(configuration.GetSection(MailgunOptions.Section));
        services.Configure<MapboxOptions>(configuration.GetSection(MapboxOptions.Section));
        services.Configure<S3StorageOptions>(configuration.GetSection(S3StorageOptions.Section));
        services.Configure<IncidentOptions>(configuration.GetSection(IncidentOptions.Section));
        services.Configure<FunderMapsOptions>(configuration.GetSection(FunderMapsOptions.Section));

        services.AddDataProtection(options =>
        {
            options.ApplicationDiscriminator = configuration["DataProtection:ApplicationName"] ?? "FunderMaps";
        });

        return services;
    }

    public static IServiceCollection AddFunderMapsAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<PasswordHasher>();
        services.AddTransient<JwtSecurityTokenService>();
        services.AddScoped<SignInService>();

        // Resolve IKeystoreRepository from the real container at runtime, not from an intermediate one.
        services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(sp =>
            new ConfigureOptions<KeyManagementOptions>(options =>
            {
                var keystoreRepository = sp.GetRequiredService<IKeystoreRepository>();
                options.XmlRepository = new KeystoreXmlRepository(keystoreRepository);
            }));

        var authBuilder = services.AddAuthentication("FunderMapsHybridAuth")
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
            .AddScheme<AuthKeyAuthenticationOptions, AuthKeyAuthenticationHandler>(AuthKeyAuthenticationOptions.DefaultScheme, options => { });

        authBuilder.AddFunderMapsScheme(AuthKeyAuthenticationOptions.DefaultScheme);

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
