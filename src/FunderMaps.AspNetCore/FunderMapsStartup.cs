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
    /// <summary>
    ///     FunderMaps.AspNetCore configuration and service setup.
    /// </summary>
    public class FunderMapsStartup : IHostingStartup
    {
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
                services.AddAutoMapper(mapper =>
                {
                    mapper.CreateMap<Organization, OrganizationDto>().ReverseMap();
                    mapper.CreateMap<User, UserDto>().ReverseMap();
                    mapper.CreateMap<User, OrganizationUserDto>().ReverseMap();
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
                    User = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item1 : null,
                    Tenant = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item2 : null,
                };
            }

            return new Core.AppContext();
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
