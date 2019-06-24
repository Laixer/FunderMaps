using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using FunderMaps.Data;
using FunderMaps.Data.Repositories;
using FunderMaps.Models.Identity;
using FunderMaps.Interfaces;
using FunderMaps.Services;
using FunderMaps.Helpers;
using FunderMaps.Extensions;
using FunderMaps.Authorization.Handler;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
using FunderMaps.Middleware;
using FunderMaps.HealthChecks;

namespace FunderMaps
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatastore(services);
            ConfigureAuthentication(services);
            ConfigureAuthorization(services);

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // In production, the frontend framework files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CORSDeveloperPolicy",
                builder =>
                {
                    builder.WithOrigins("https://localhost:8080",
                                        "http://localhost:8080")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();

                    var domain = _configuration.GetDomainHost();
                    if (domain != null)
                    {
                        builder.WithOrigins($"https://{domain}");
                    }
                });
            });

            // Enable response compression.
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddHealthChecks().AddCheck<ApiHealthCheck>("api_health_check");

            // Register the Swagger generator, defining an OpenAPI document.
            services.AddSwaggerDocumentation();

            // Configure local repositories
            ConfigureRepository(services);

            services.AddTransient<IFileStorageService, AzureBlobStorageService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IAddressService, AddressService>();
        }

        /// <summary>
        /// Setup local repositories and register them with the service collector.
        /// </summary>
        /// <param name="services">Service collection.</param>
        private void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<IPrincipalRepository, PrincipalRepository>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        }

        /// <summary>
        /// Setup various data stores for entities.
        /// </summary>
        /// <param name="services">Service collection.</param>
        private void ConfigureDatastore(IServiceCollection services)
        {
            // Application database
            services.AddDbContextPool<FunderMapsDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("FunderMapsConnection"), pgOptions =>
                {
                    pgOptions.MigrationsHistoryTable("migrations_history", "meta");
                    pgOptions.EnableRetryOnFailure();
                    pgOptions.UseNetTopologySuite();
                });
            })
            .AddEntityFrameworkNpgsql();

            // FIS database
            services.AddDbContextPool<FisDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("FISConnection"));
            })
            .AddEntityFrameworkNpgsql();
        }

        /// <summary>
        /// Configure the identity framework and the authentications methods.
        /// </summary>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentity<FunderMapsUser, FunderMapsRole>(options =>
            {
                // Password settings.
                options.Password = Constants.PasswordPolicy;

                // Lockout settings.
                options.Lockout = Constants.LockoutOptions;

                // User settings.
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<FunderMapsDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _configuration.GetJwtIssuer(),
                    ValidAudience = _configuration.GetJwtAudience(),
                    IssuerSigningKey = _configuration.GetJwtSignKey(),
                };
            });
        }

        /// <summary>
        /// Configure the authorization policies.
        /// </summary>
        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OrganizationMemberPolicy",
                    policy => policy.AddRequirements(new OrganizationMemberRequirement()));
                options.AddPolicy("OrganizationSuperuserPolicy",
                    policy => policy.AddRequirements(new OrganizationRoleRequirement(Constants.SuperuserRole)));
            });

            services.AddScoped<IAuthorizationHandler, OrganizationMemberHandler>();
            services.AddScoped<IAuthorizationHandler, OrganizationRoleHandler>();
            services.AddSingleton<IAuthorizationHandler, FisOperationHandler>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this  method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                app.UseSwaggerDocumentation();
                app.UseCors("CORSDeveloperPolicy");
            }
            if (env.IsStaging())
            {
                app.UseSwaggerDocumentation();
                app.UseCors("CORSDeveloperPolicy");
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/oops");
                app.UseHsts();
                app.UseEnhancedSecurity();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", $"public, max-age={Constants.StaticFileCacheRetention}");
                }
            });
            app.UseSpaStaticFiles();

            app.UseHealthChecks("/health");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "oops",
                    template: "oops",
                    defaults: new { controller = "Error", action = "Error" });
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
