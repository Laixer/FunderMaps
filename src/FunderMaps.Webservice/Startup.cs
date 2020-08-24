using AutoMapper;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Documentation;
using FunderMaps.Webservice.HealthChecks;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO.Compression;

[assembly: ApiController]
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

            // TODO: REVIEW: Should we IgnoreNullValues ?
            services.AddControllers();

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

            // Configure Swagger.
            services.AddSwaggerGen(c =>
            {
                // FUTURE: The full enum description support for swagger with System.Text.Json is a WIP. This is a custom tempfix.
                c.SchemaFilter<EnumSchemaFilter>();
                c.GeneratePolymorphicSchemas();
            });
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
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FunderMaps Webservice");
                c.RoutePrefix = string.Empty;
            });

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
