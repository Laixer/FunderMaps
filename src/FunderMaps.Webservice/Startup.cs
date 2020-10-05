using AutoMapper;
using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Services;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Documentation;
using FunderMaps.Webservice.Handlers;
using FunderMaps.AspNetCore.Helpers;
using FunderMaps.Webservice.HealthChecks;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using FunderMaps.Extensions;
using FunderMaps.AspNetCore.Authentication;

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
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddControllers()
                .AddFunderMapsAssembly();

            // Configure project specific services.
            services.AddTransient<IMappingService, MappingService>();
            services.AddTransient<ProductHandler>();
            services.AddTransient<SignInHandler>();

            // Configure FunderMaps services.
            services.AddFunderMapsDataServices("FunderMapsConnection");

            // Override default product service by tracking variant of product service.
            services.Replace(ServiceDescriptor.Transient<IProductService, ProductTrackingService>());

            // Configure health checks.
            services.AddHealthChecks()
                .AddCheck<WebserviceHealthCheck>("webservice_health_check");

            // Configure Swagger.
            services.AddSwaggerGen(c =>
            {
                // FUTURE: The full enum description support for swagger with System.Text.Json is a WIP. This is a custom tempfix.
                c.SchemaFilter<EnumSchemaFilter>();
                // FUTURE: This call is obsolete.
                c.GeneratePolymorphicSchemas();
            });

            // Add the authentication layer.
            services.AddFunderMapsCoreAuthentication();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetJwtIssuer(),
                        ValidAudience = Configuration.GetJwtAudience(),
                        IssuerSigningKey = JwtHelper.CreateSecurityKey(Configuration.GetJwtSigningKey()), // TODO: Only for testing
                    };
                })
                .AddJwtBearerTokenProvider();

            // Add the authorization layer.
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddFunderMapsPolicy();
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
            if (env.IsProduction())
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/oops");
            }

            app.UseFunderMapsExceptionHandler("/oops");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FunderMaps Webservice");
                c.RoutePrefix = string.Empty;
            });

            app.UsePathBase(new PathString("/api"));
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
