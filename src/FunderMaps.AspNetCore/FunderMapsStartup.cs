using System;
using System.Linq;
using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: HostingStartup(typeof(FunderMaps.AspNetCore.FunderMapsStartup))]

namespace FunderMaps.AspNetCore
{
    public class FunderMapsStartup : IHostingStartup
    {
        private Core.AppContext AppContextFactory(IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            if (httpContextAccessor.HttpContext != null)
            {
                HttpContext httpContext = httpContextAccessor.HttpContext;
                return new Core.AppContext
                {
                    CancellationToken = httpContext.RequestAborted,
                    Items = new System.Collections.Generic.Dictionary<object, object>(httpContext.Items),
                    ServiceProvider = httpContext.RequestServices,
                    User = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetTenantUser<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item1 : null,
                    Tenant = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetTenantUser<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item2 : null,
                };
            }

            return new Core.AppContext();
        }

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

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAutoMapper(mapper =>
                {
                    mapper.CreateMap<Organization, OrganizationDto>().ReverseMap();
                    mapper.CreateMap<User, UserDto>().ReverseMap();
                });

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
    }
}
