using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.HealthChecks;
using FunderMaps.AspNetCore.Middleware;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.Core.ExternalServices.Mailgun;
using FunderMaps.Core.ExternalServices.Mapbox;
using FunderMaps.Core.ExternalServices.OpenAI;
using FunderMaps.Core.ExternalServices.S3Storage;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Data.Providers;
using FunderMaps.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsAspNetCoreServiceCollectionExtensions
{
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
            .AddCheck<MapboxHealthCheck>("mapbox_health_check", tags: new[] { "extern" })
            .AddCheck<RepositoryHealthCheck>("data_health_check", tags: new[] { "extern" })
            .AddCheck<EmailHealthCheck>("email_health_check", tags: new[] { "extern" })
            .AddCheck<BlobStorageHealthCheck>("blob_storage_health_check", tags: new[] { "extern" })
            .AddCheck<IOHealthCheck>("io_health_check", tags: new[] { "local" })
            .AddCheck<TippecanoeHealthCheck>("tileset_generator_health_check", tags: new[] { "local" });

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

    public static IServiceCollection AddFunderMapsAspNetCoreAuth(this IServiceCollection services)
    {
        services.AddScoped<SignInService>();
        services.AddTransient<ISecurityTokenProvider, JwtBearerTokenProvider>();

        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.TokenValidationParameters = new JwtTokenValidationParameters
                {
                    ValidIssuer = configuration.GetJwtIssuer(),
                    ValidAudience = configuration.GetJwtAudience(),
                    IssuerSigningKey = configuration.GetJwtSigningKey(),
                    Valid = configuration.GetJwtTokenExpirationInMinutes(),
                };
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

    public static IServiceCollection AddFunderMapsAspNetCoreControllers(this IServiceCollection services)
    {
        // FUTURE: Only load specific parts.
        // NOTE: This will register all controllers in the FunderMaps.AspNetCore
        //       assemly regardless of authentication and authorization.
        services.AddControllers(options => options.Filters.Add(typeof(FunderMapsCoreExceptionFilter))).AddFunderMapsAssembly();

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
