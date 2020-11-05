using AutoMapper.Configuration;
using FunderMaps.Core.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Application configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="configuration">See <see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration) => Configuration = configuration;

        /// <summary>
        ///     This method gets called by the runtime if no environment is set.
        /// </summary>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure FunderMaps services.
            services.AddFunderMapsCoreServices();
            services.AddFunderMapsInfrastructureServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");

            // Add all types of background tasks as an enumerable.
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(Jobs.BundleBuilder.Job)),
                ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(Jobs.DummyCommand)),
            });

            services.AddGrpc();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP
        ///     request pipeline if no environment is set.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ItemEnqueueService>();
                endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
            });
        }
    }
}
