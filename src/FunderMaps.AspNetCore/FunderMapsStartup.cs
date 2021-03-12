using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.HealthChecks;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
        ///     Use this method to add entity and object mapping configurations.
        /// </summary>
        private static void ConfigureMapper(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Address, AddressBuildingDto>()
                .IncludeMembers(src => src.BuildingNavigation)
                .ForMember(dest => dest.AddressId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.BuildingId, o => o.MapFrom(src => src.BuildingNavigation.Id))
                .ForMember(dest => dest.BuildingGeometry, o => o.MapFrom(src => src.BuildingNavigation.Geometry));

            mapper.CreateMap<AnalysisProduct, AnalysisFoundationDto>();
            mapper.CreateMap<AnalysisProduct, AnalysisCompleteDto>();
            mapper.CreateMap<AnalysisProduct, AnalysisRiskPlusDto>();
            mapper.CreateMap<Building, AddressBuildingDto>().ReverseMap();
            mapper.CreateMap<Contact, IncidentDto>().ReverseMap();
            mapper.CreateMap<Incident, IncidentDto>()
                .IncludeMembers(src => src.ContactNavigation)
                .ReverseMap();
            mapper.CreateMap<Organization, OrganizationDto>().ReverseMap();
            mapper.CreateMap<StatisticsProduct, StatisticsDto>();
            mapper.CreateMap<TokenContext, SignInSecurityTokenDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Token.Id))
                .ForMember(dest => dest.Issuer, o => o.MapFrom(src => src.Token.Issuer))
                .ForMember(dest => dest.Token, o => o.MapFrom(src => src.TokenString))
                .ForMember(dest => dest.ValidFrom, o => o.MapFrom(src => src.Token.ValidFrom))
                .ForMember(dest => dest.ValidTo, o => o.MapFrom(src => src.Token.ValidTo));
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
                services.AddAutoMapper(mapper => ConfigureMapper(mapper));

                // FUTURE: Only load specific parts.
                // NOTE: This will register all controllers in the FunderMaps.AspNetCore
                //       assemly regardless of authentication and authorization.
                services.AddControllers()
                    .AddFunderMapsAssembly();

                // Register components from reference assemblies.
                services.AddFunderMapsCoreServices();

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
