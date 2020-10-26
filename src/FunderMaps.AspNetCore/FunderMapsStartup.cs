using System;
using System.Linq;
using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        private void ConfigureMapper(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Address, AddressBuildingDto>()
                .IncludeMembers(src => src.BuildingNavigation)
                .ForMember(dest => dest.AddressId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.BuildingId, o => o.MapFrom(src => src.BuildingNavigation.Id))
                .ForMember(dest => dest.BuildingGeometry, o => o.MapFrom(src => src.BuildingNavigation.Geometry));
            mapper.CreateMap<AnalysisProduct, AnalysisRiskDto>();
            mapper.CreateMap<Building, AddressBuildingDto>().ReverseMap();
            mapper.CreateMap<Contact, IncidentDto>().ReverseMap();
            mapper.CreateMap<Incident, IncidentDto>()
                .IncludeMembers(src => src.ContactNavigation)
                .ReverseMap();
            mapper.CreateMap<Organization, OrganizationDto>().ReverseMap();
            mapper.CreateMap<TokenContext, SignInSecurityTokenDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Token.Id))
                .ForMember(dest => dest.Issuer, o => o.MapFrom(src => src.Token.Issuer))
                .ForMember(dest => dest.Token, o => o.MapFrom(src => src.TokenString))
                .ForMember(dest => dest.ValidFrom, o => o.MapFrom(src => src.Token.ValidFrom))
                .ForMember(dest => dest.ValidTo, o => o.MapFrom(src => src.Token.ValidTo));
            mapper.CreateMap<User, UserDto>().ReverseMap();
            mapper.CreateMap<User, OrganizationUserDto>().ReverseMap();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="builder">Extend the instance of <see cref="IWebHostBuilder"/>.</param>
        public void Configure(IWebHostBuilder builder)
        {
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
                services.AddFunderMapsExceptionMapper();

                // NOTE: Register the HttpContextAccessor service to the container.
                //       The HttpContextAccessor exposes a singleton holding the
                //       HttpContext within a scoped resolver, or null outside the scope.
                //       Some components require the HttpContext and its features when the
                //       related service is being resolved within the scope.
                services.AddHttpContextAccessor();

                AddAppContext(services);
            });
        }

        // TODO: Move this into a singleton service.
        private Core.AppContext AppContextFactory(IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            if (httpContextAccessor.HttpContext is null)
            {
                return new Core.AppContext();
            }

            HttpContext httpContext = httpContextAccessor.HttpContext;
            return new Core.AppContext
            {
                CancellationToken = httpContext.RequestAborted,
                Items = new System.Collections.Generic.Dictionary<object, object>(httpContext.Items),
                Cache = httpContext.RequestServices.GetRequiredService<IMemoryCache>(),
                ServiceProvider = httpContext.RequestServices,
                User = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item1 : null,
                Tenant = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item2 : null,
            };
        }

        // FUTURE: Create a service replace method from this stub.
        /// <summary>
        ///     Replace the stock <see cref="AppContext" which an application context
        ///     hooked on the ASP.NET Core framework. The integration couples front framework
        ///     operations to the application core without imparing assemly dependencies.
        /// </summary>
        /// <param name="services">Collection of DI services.</param>
        public IServiceCollection AddAppContext(IServiceCollection services)
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(Core.AppContext), AppContextFactory, ServiceLifetime.Scoped);

            var hasAppContextService = services.Any(d => d.ServiceType == typeof(Core.AppContext));
            if (hasAppContextService)
            {
                services.Replace(serviceDescriptor);
            }
            else
            {
                services.Add(serviceDescriptor);
            }

            return services;
        }
    }
}
