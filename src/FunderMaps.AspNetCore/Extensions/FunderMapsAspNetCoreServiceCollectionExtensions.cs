using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.HealthChecks;
using FunderMaps.AspNetCore.Middleware;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Services;
using FunderMaps.Core.Storage;
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
    /// <summary>
    ///     Use this method to add entity and object mapping configurations.
    /// </summary>
    private static void ConfigureMapper(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Address, AddressDto>();
        mapper.CreateMap<Organization, OrganizationDto>()
            .ForMember(dest => dest.XMin, o => o.MapFrom(src => src.Area.XMin))
            .ForMember(dest => dest.YMin, o => o.MapFrom(src => src.Area.YMin))
            .ForMember(dest => dest.XMax, o => o.MapFrom(src => src.Area.XMax))
            .ForMember(dest => dest.YMax, o => o.MapFrom(src => src.Area.YMax))
            .ForMember(dest => dest.CenterX, o => o.MapFrom(src => src.Center.CenterX))
            .ForMember(dest => dest.CenterY, o => o.MapFrom(src => src.Center.CenterY))
            .ReverseMap();
        mapper.CreateMap<TokenContext, SignInSecurityTokenDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Token.Id))
            .ForMember(dest => dest.Issuer, o => o.MapFrom(src => src.Token.Issuer))
            .ForMember(dest => dest.Token, o => o.MapFrom(src => src.TokenString))
            .ForMember(dest => dest.ValidFrom, o => o.MapFrom(src => src.Token.ValidFrom))
            .ForMember(dest => dest.ValidTo, o => o.MapFrom(src => src.Token.ValidTo));
        mapper.CreateMap<User, UserDto>().ReverseMap();
        mapper.CreateMap<User, OrganizationUserDto>().ReverseMap();
    }

    public static IServiceCollection AddFunderMapsAspNetCoreServices(this IServiceCollection services)
    {
        services.AddFunderMapsCoreServices();
        services.AddFunderMapsDataServices();

        services.AddScoped<FunderMapsClient>();

        // NOTE: Register the HttpContextAccessor service to the container.
        //       The HttpContextAccessor exposes a singleton holding the
        //       HttpContext within a scoped resolver, or null outside the scope.
        //       Some components require the HttpContext and its features when the
        //       related service is being resolved within the scope.
        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddCheck<MapboxHealthCheck>("mapbox_health_check")
            .AddCheck<IOHealthCheck>("io_health_check")
            .AddCheck<RepositoryHealthCheck>("data_health_check")
            .AddCheck<EmailHealthCheck>("email_health_check")
            .AddCheck<BlobStorageHealthCheck>("blob_storage_health_check");

        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        // Configure services with configuration.
        // Any application depending on ASP.NET Core should have an IConfiguration service registered.
        services.Configure<MailgunOptions>(configuration.GetSection(MailgunOptions.Section));
        services.Configure<MapboxOptions>(configuration.GetSection(MapboxOptions.Section));
        services.Configure<BlobStorageOptions>(configuration.GetSection(BlobStorageOptions.Section));
        services.Configure<IncidentOptions>(configuration.GetSection(IncidentOptions.Section));

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
        services.AddAutoMapper(mapper => ConfigureMapper(mapper));

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
