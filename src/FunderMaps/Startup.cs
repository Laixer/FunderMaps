using FunderMaps.Authorization;
using FunderMaps.Core.Interfaces;
using FunderMaps.Data;
using FunderMaps.Data.Repositories;
using FunderMaps.Event;
using FunderMaps.Event.Handlers;
using FunderMaps.Extensions;
using FunderMaps.HealthChecks;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.Middleware;
using FunderMaps.Models.Identity;
using FunderMaps.Services;
using Laixer.Identity.Dapper.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;

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
        public Startup(IConfiguration configuration) => _configuration = configuration;

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddDbProvider("FunderMapsConnection");

            ConfigureAuthentication(services);
            ConfigureAuthorization(services);

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddControllers();

            // In production, the frontend framework files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Set CORS policy.
            services.AddCorsPolicy(_configuration);

            // Enable response compression.
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("api_health_check")
                .AddCheck<DatabaseHealthCheck>("db_health_check")
                .AddCheck<FileStorageCheck>("file_health_check");

            services.AddEventBus()
                .AddHandler<IUpdateUserProfileEvent, UpdateUserProfileHandler>();

            // Configure local repositories
            ConfigureRepository(services);

            // Register services from application modules.
            services.AddApplicationCoreServices(_configuration);
            services.AddApplicationCloudServices(_configuration);

            // Register services from local application module.
            services.AddTransient<IMailService, MailService>();
        }

        /// <summary>
        /// Setup local repositories and register them with the service collector.
        /// </summary>
        /// <param name="services">Service collection.</param>
        private static void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddScoped<IOrganizationProposalRepository, OrganizationProposalRepository>();
            services.AddScoped<IFoundationRecoveryRepository, FoundationRecoveryRepository>();
            services.AddScoped<IMapRepository, MapRepository>();
        }

        /// <summary>
        /// Configure the identity framework and the authentications methods.
        /// </summary>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentity<FunderMapsUser, FunderMapsRole>(options =>
            {
                options.Password = Constants.PasswordPolicy;
                options.Lockout = Constants.LockoutOptions;
                options.User.RequireUniqueEmail = true;
            })
            .AddDapperStores(options =>
            {
                options.UserTable = "user";
                options.Schema = "application";
                options.MatchWithUnderscore = true;
                options.UseNpgsql<FunderMapsCustomQuery>(_configuration.GetConnectionStringFallback("FunderMapsConnection"));
            })
            .AddDefaultTokenProviders();

            // FUTURE: Replace with AddIdentityServer
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
        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var organizationMemberPolicyBuilder = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.OrganizationUser);

                options.AddPolicy(Constants.OrganizationMemberPolicy, organizationMemberPolicyBuilder
                    .RequireClaim(ClaimTypes.OrganizationUserRole)
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberWritePolicy, organizationMemberPolicyBuilder
                    .RequireAssertion(context => context.User.HasOrganization() &&
                        (context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Verifier ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Writer))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberVerifyPolicy, organizationMemberPolicyBuilder
                    .RequireAssertion(context => context.User.HasOrganization() &&
                        (context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Verifier))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberSuperPolicy, organizationMemberPolicyBuilder
                    .RequireAssertion(context => context.User.HasOrganization() &&
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser)
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.FindFirst(ClaimTypes.OrganizationUserRole) != null) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberWriteOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Verifier ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Writer) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberVerifyOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Verifier) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberSuperOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.GetOrganizationRole() == Core.Entities.OrganizationRole.Superuser) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this  method to configure the HTTP request pipeline.
        /// </summary>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors("CORSDeveloperPolicy");
            }
            if (env.IsStaging())
            {
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

            var staticFileOptions = new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    // Static files can be heavily cached since they can be large and do not change often.
                    context.Context.Response.Headers.Add("Cache-Control", $"public, max-age={Constants.StaticFileCacheRetention}");
                }
            };

            app.UseStaticFiles(staticFileOptions);
            app.UseSpaStaticFiles(staticFileOptions);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
