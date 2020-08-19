using AutoMapper;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.HealthChecks;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO.Compression;

namespace FunderMaps.Webservice
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
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }

            //services.AddApplicationInsightsTelemetry();

            // TODO: REVIEW: Should we IgnoreNullValues ?
            services.AddControllers();

            services.AddResponseCompression(options =>
            {
                // NOTE: Compression is disabled by default when serving data
                // over HTTPS because of BREACH exploit.
                options.EnableForHttps = true;
            });

            // Configure services.
            services.AddTransient<IDescriptionService, DescriptionService>();
            services.AddTransient<IMappingService, MappingService>();
            services.AddTransient<IProductRequestService, ProductRequestService>();
            services.AddTransient<IProductResultService, ProductResultService>();
            services.AddTransient<IProductService, ProductService>();

            // Configure FunderMaps services.
            services.AddFunderMapsDataServices("FunderMapsConnection");

            // Configure AutoMapper.
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // Configure health checks.
            services.AddHealthChecks()
                .AddCheck<WebserviceHealthCheck>("webservice_health_check");

            // Configure compression providers.
            services
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest)
                .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this  method to configure the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/oops");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // TODO: Set /api endpoint as default
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
