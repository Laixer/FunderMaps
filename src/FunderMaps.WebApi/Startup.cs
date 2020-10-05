using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Extensions;
using FunderMaps.HealthChecks;
using FunderMaps.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

[assembly: ApiController]
namespace FunderMaps.WebApi
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
            services.AddAutoMapper(typeof(Startup));

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

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("api_health_check")
                //.AddCheck<DatabaseHealthCheck>("db_health_check")
                .AddCheck<FileStorageCheck>("file_health_check");

            // Register components from reference assemblies.
            services.AddFunderMapsInfrastructureServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");

            services.AddTransient<AuthenticationHelper>();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this  method to configure the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UsePathBase(new PathString("/api"));
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // FUTURE: Should be created via factory
            app.Use(async (context, next) =>
            {
                var appContext = context.RequestServices.GetRequiredService<Core.AppContext>();
                appContext.CancellationToken = context.RequestAborted;
                appContext.Items = new System.Collections.Generic.Dictionary<object, object>(context.Items);
                appContext.ServiceProvider = context.RequestServices;
                appContext.User = Core.Authentication.PrincipalProvider.IsSignedIn(context.User) ? Core.Authentication.PrincipalProvider.GetTenantUser<Core.Entities.User, Core.Entities.Organization>(context.User).Item1 : null;
                appContext.Tenant = Core.Authentication.PrincipalProvider.IsSignedIn(context.User) ? Core.Authentication.PrincipalProvider.GetTenantUser<Core.Entities.User, Core.Entities.Organization>(context.User).Item2 : null;
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
