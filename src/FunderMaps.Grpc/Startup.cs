using FunderMaps.Core.Managers;
using FunderMaps.Core.Types.BackgroundTasks;
using FunderMaps.Grpc.DummyTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Grpc
{
    /// <summary>
    ///     Configures our service collection and request/response pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configure our services.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure FunderMaps services.
            services.AddFunderMapsCoreServices();
            services.AddFunderMapsInfrastructureServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");

            // TODO This will be added to the core extension.
            // Configure background worker
            services.AddSingleton<QueueManager>();

            // TODO Remove
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient(typeof(BackgroundTaskBase), typeof(AsyncDummyTask)),
            });

            services.AddGrpc();
        }

        /// <summary>
        ///     Configure our request/response pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ItemEnqueueService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hi");
                });
            });
        }
    }
}
