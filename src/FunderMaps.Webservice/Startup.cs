using System;
using System.IO;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Webservice.Documentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        ///     Use this method to add services to the container regardless of the environment.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        private void StartupConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "FunderMaps Webservice",
                    Version = "v1",
                    Description = "FunderMaps Webservice REST API",
                    Contact = new()
                    {
                        Name = "Laixer B.V.",
                        Email = "info@laixer.com",
                    },
                }
                );
                options.DocumentFilter<BasePathFilter>();

                // FUTURE: The full enum description support for swagger with System.Text.Json is a WIP. This is a custom tempfix.
                options.SchemaFilter<EnumSchemaFilter>();
                options.UseOneOfForPolymorphism();

                string DocumentationFile = $"{AppContext.BaseDirectory}DocumentationFunderMapsWebservice.xml";
                if (File.Exists(DocumentationFile))
                {
                    options.IncludeXmlComments(DocumentationFile);
                }
            });

            // Register components from reference assemblies.
            services.AddFunderMapsDataServices("FunderMapsConnection");
        }

        /// <summary>
        ///     This method gets called by the runtime if no environment is set.
        /// </summary>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfigureServices(services);
        }

        /// <summary>
        ///     This method gets called by the runtime if environment is set to development.
        /// </summary>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            StartupConfigureServices(services);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP
        ///     request pipeline if environment is set to development.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public static void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors();

            app.UseExceptionHandler("/oops");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FunderMaps Webservice");
                options.RoutePrefix = string.Empty;
            });

            app.UsePathBase(new("/api"));
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAspAppContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP
        ///     request pipeline if environment is set to staging.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public static void ConfigureStaging(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            app.UseExceptionHandler("/oops");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FunderMaps Webservice");
                options.RoutePrefix = string.Empty;
            });

            app.UsePathBase(new("/api"));
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAspAppContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP
        ///     request pipeline if no environment is set.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            app.UseExceptionHandler("/oops");

            app.UseSwagger();

            app.UsePathBase(new("/api"));
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAspAppContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
            });
        }
    }
}
