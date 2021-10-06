using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.HealthChecks;
using FunderMaps.AspNetCore.Middleware;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ApiController]
[assembly: HostingStartup(typeof(FunderMaps.AspNetCore.FunderMapsStartup))]
namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     FunderMaps.AspNetCore configuration and service setup.
    /// </summary>
    public class FunderMapsStartup : IHostingStartup
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
        ///     Use this method to add entity and object mapping configurations.
        /// </summary>
        private static void ConfigureMapper(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Address, AddressDto>();
            mapper.CreateMap<AnalysisProduct, AnalysisFoundationDto>();
            mapper.CreateMap<AnalysisProduct, AnalysisCompleteDto>();
            mapper.CreateMap<AnalysisProduct, AnalysisRiskPlusDto>();
            mapper.CreateMap<AnalysisProduct2, AnalysisV2Dto>();
            mapper.CreateMap<Contact, IncidentDto>().ReverseMap();
            mapper.CreateMap<Incident, IncidentDto>()
                .IncludeMembers(src => src.ContactNavigation)
                .ReverseMap();
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
            mapper.CreateMap<StatisticsProduct, StatisticsDto>();
            mapper.CreateMap<User, UserDto>().ReverseMap();
            mapper.CreateMap<User, OrganizationUserDto>().ReverseMap();

            // FUTURE: Rewrite.
            mapper.CreateMap<Years, YearsResponseModel>();
            mapper.CreateMap<ConstructionYearDistribution, ConstructionYearDistributionResponseModel>();
            mapper.CreateMap<ConstructionYearPair, ConstructionYearPairResponseModel>();
            mapper.CreateMap<FoundationRiskDistribution, FoundationRiskDistributionResponseModel>();
            mapper.CreateMap<FoundationTypeDistribution, FoundationTypeDistributionResponseModel>();
            mapper.CreateMap<FoundationTypePair, FoundationTypePairResponseModel>();
            mapper.CreateMap<IncidentYearPair, IncidentYearPairResponseModel>();
            mapper.CreateMap<InquiryYearPair, InquiryYearPairResponseModel>();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="builder">Extend the instance of <see cref="IWebHostBuilder"/>.</param>
        public void Configure([DisallowNull] IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ConfigureServices(services =>
            {
                // The startup essential properties can be used to setup components.
                (Configuration, HostEnvironment) = services.BuildStartupProperties();

                services.AddAutoMapper(mapper => ConfigureMapper(mapper));

                // FUTURE: Only load specific parts.
                // NOTE: This will register all controllers in the FunderMaps.AspNetCore
                //       assemly regardless of authentication and authorization.
                services.AddControllers(options => options.Filters.Add(typeof(FunderMapsCoreExceptionFilter)))
                    .AddFunderMapsAssembly();

                // Register components from reference assemblies.
                services.AddFunderMapsCoreServices();

                {
                    // Add the authentication layer.
                    services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.SaveToken = false;
                            options.TokenValidationParameters = new JwtTokenValidationParameters
                            {
                                ValidIssuer = Configuration.GetJwtIssuer(),
                                ValidAudience = Configuration.GetJwtAudience(),
                                IssuerSigningKey = Configuration.GetJwtSigningKey(),
                                Valid = Configuration.GetJwtTokenExpirationInMinutes(),
                            };
                        });

                    // Add the authorization layer.
                    services.AddAuthorization(options =>
                        {
                            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                            options.AddFunderMapsPolicy();
                        });
                }

                // Adds the core authentication service to the container.
                services.AddScoped<SignInService>();
                services.AddTransient<ISecurityTokenProvider, JwtBearerTokenProvider>();

                // NOTE: Register the HttpContextAccessor service to the container.
                //       The HttpContextAccessor exposes a singleton holding the
                //       HttpContext within a scoped resolver, or null outside the scope.
                //       Some components require the HttpContext and its features when the
                //       related service is being resolved within the scope.
                services.AddHttpContextAccessor();

                services.AddHealthChecks()
                    .AddCheck<IOHealthCheck>("io_health_check")
                    .AddCheck<RepositoryHealthCheck>("data_health_check")
                    .AddCheck<EmailHealthCheck>("email_health_check")
                    .AddCheck<BlobStorageHealthCheck>("blob_storage_health_check");
            });
        }
    }
}
