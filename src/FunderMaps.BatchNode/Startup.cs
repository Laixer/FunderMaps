using FunderMaps.AspNetCore;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Add batch jobs to the DI container.
            services.AddBatchJob<Jobs.BundleBuilder.BundleBatch>();
            services.AddBatchJob<Jobs.BundleBuilder.BundleJob>();
            services.AddBatchJob<Jobs.Notification.EmailJob>();
            services.AddBatchJob<Jobs.IncidentNotifyJob>();

            services.AddGrpc();

            // NOTE: Register the HttpContextAccessor service to the container.
            //       The HttpContextAccessor exposes a singleton holding the
            //       HttpContext within a scoped resolver, or null outside the scope.
            //       Some components require the HttpContext and its features when the
            //       related service is being resolved within the scope.
            services.AddHttpContextAccessor();

            // Add the task scheduler.
            services.AddHostedService<TimedHostedService>();

            services.AddOrReplace<IAppContextFactory, GrpcAppContextFactory>(ServiceLifetime.Singleton);
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
            app.UseForwardedHeaders(new()
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
                endpoints.MapGrpcService<BatchService>();
            });
        }
    }
}
