using AutoMapper;
using FunderMaps.Core.Authentication;
using FunderMaps.Extensions;
using FunderMaps.HealthChecks;
using FunderMaps.WebApi.Authentication;
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
        ///     This method gets called by the runtime. Use this method to add production services to the container.
        /// </summary>
        /// <remarks>
        ///     Order is undetermined when configuring services.
        /// </remarks>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
        }

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

            ConfigureAuthentication(services);
            ConfigureAuthorization(services);

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

            services.AddTransient<AuthenticationHelper>();
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
                        IssuerSigningKey = JwtHelper.CreateSecurityKey(Configuration.GetJwtSigningKey()), // TODO: Only for testing
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
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Authorization policy matrix
                //                              || ApplicationRole                || OrganizationRole
                //                              || Administrator   | User | Guest || Superuser | Verifier | Writer | Reader
                // AdministratorPolicy          || Yes             | No   | No    || No        | No       | No     | No
                // SuperuserAdministratorPolicy || Yes             | No   | No    || Yes       | No       | No     | No
                // SuperuserPolicy              || Yes             | Yes  | Yes   || Yes       | No       | No     | No
                // VerifierAdministratorPolicy  || Yes             | No   | No    || Yes       | Yes      | No     | No
                // VerifierPolicy               || Yes             | Yes  | Yes   || Yes       | Yes      | No     | No
                // WriterAdministratorPolicy    || Yes             | No   | No    || Yes       | Yes      | Yes    | No
                // WriterPolicy                 || Yes             | Yes  | Yes   || Yes       | Yes      | Yes    | No
                // ReaderAdministratorPolicy    || Yes             | No   | No    || Yes       | Yes      | Yes    | Yes
                // ReaderPolicy                 || Yes             | Yes  | Yes   || Yes       | Yes      | Yes    | Yes

                options.AddPolicy("AdministratorPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Core.Types.ApplicationRole.Administrator.ToString()));

                options.AddPolicy("SuperuserAdministratorPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                    }));

                options.AddPolicy("SuperuserPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString());
                    }));

                options.AddPolicy("VerifierAdministratorPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                               context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                    }));

                options.AddPolicy("VerifierPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString());
                    }));

                options.AddPolicy("WriterAdministratorPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                               context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                    }));

                options.AddPolicy("WriterPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Writer.ToString());
                    }));

                options.AddPolicy("ReaderAdministratorPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Reader.ToString()) ||
                               context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                    }));

                options.AddPolicy("ReaderPolicy", policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                               context.User.HasClaim(FunderMapsAuthenticationClaimTypes.OrganizationRole, Core.Types.OrganizationRole.Reader.ToString());
                    }));
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

                app.UseResponseCompression();
                app.UseHttpsRedirection();
            }

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
