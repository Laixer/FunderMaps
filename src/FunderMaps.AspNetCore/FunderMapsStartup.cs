using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FunderMaps.AspNetCore.FunderMapsStartup))]

namespace FunderMaps.AspNetCore
{
    public class FunderMapsStartup : IHostingStartup
    {
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
                // NOTE: This will register all controllers in the 
                //       FunderMaps.AspNetCore assemly regardless of
                //       authentication and authorization.
                services.AddControllers()
                    .AddFunderMapsAssembly();

                // Register components from reference assemblies.
                services.AddFunderMapsCoreServices();
                services.AddFunderMapsExceptionMapper();
            });
        }
    }
}
