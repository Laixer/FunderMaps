using AutoMapper;
using FunderMaps.Authorization;
using FunderMaps.Extensions;
using FunderMaps.HealthChecks;
using FunderMaps.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

// TODO: Register assembly as api controller
namespace FunderMaps
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
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper(typeof(Startup));

            ConfigureAuthentication(services);
            //ConfigureAuthorization(services);

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddControllers(); // TODO: REVIEW: Should we IgnoreNullValues ?

            services.AddResponseCompression(options =>
            {
                // NOTE: Compression is disabled by default when serving data
                // over HTTPS because of BREACH exploit.
                options.EnableForHttps = true;
            });

            // Configure compression providers.
            services
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest)
                .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("api_health_check")
                //.AddCheck<DatabaseHealthCheck>("db_health_check")
                .AddCheck<FileStorageCheck>("file_health_check");

            // Register components from reference assemblies.
            services.AddFunderMapsCoreServices();
            //services.AddFunderMapsCloudServices();
            services.AddFunderMapsDataServices("FunderMapsConnection");
        }

        /// <summary>
        ///     Configure the identity framework and the authentications methods.
        /// </summary>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddFunderMapsCoreAuthentication();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetJwtIssuer(),
                        ValidAudience = Configuration.GetJwtAudience(),
                        IssuerSigningKey = JwtHelper.CreateSecurityKey(Configuration.GetJwtSigningKey()),
                    };
                })
                .AddJwtBearerTokenProvider();
        }

        /// <summary>
        ///     Configure the authorization policies.
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
                        (context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Verifier ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Writer))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberVerifyPolicy, organizationMemberPolicyBuilder
                    .RequireAssertion(context => context.User.HasOrganization() &&
                        (context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Verifier))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberSuperPolicy, organizationMemberPolicyBuilder
                    .RequireAssertion(context => context.User.HasOrganization() &&
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser)
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
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Verifier ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Writer) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberVerifyOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser ||
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Verifier) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());

                options.AddPolicy(Constants.OrganizationMemberSuperOrAdministratorPolicy, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => ((context.User.HasOrganization() &&
                        context.User.FindFirst(ClaimTypes.OrganizationUser) != null &&
                        context.User.GetOrganizationRole() == Core.Types.OrganizationRole.Superuser) ||
                        context.User.IsInRole(Constants.AdministratorRole)))
                    .Build());
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this  method to configure the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        ///     The order in which the pipeline handles request is of importance.
        /// </remarks>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/oops");
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
