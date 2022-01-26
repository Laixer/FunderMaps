using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

namespace FunderMaps.Admin;

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
    private static void StartupConfigureServices(IServiceCollection services)
    {
        // Register components from reference assemblies.
        services.AddFunderMapsCoreServices();
        services.AddFunderMapsInfrastructureServices();
        services.AddFunderMapsDataServices("FunderMapsConnection");

        {
            // Add the authentication layer.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
            });
            // .AddJwtBearer(options =>
            // {
            //     options.SaveToken = false;
            //     options.TokenValidationParameters = new AspNetCore.Authentication.JwtTokenValidationParameters
            //     {
            //         ValidIssuer = Configuration.GetJwtIssuer(),
            //         ValidAudience = Configuration.GetJwtAudience(),
            //         IssuerSigningKey = Configuration.GetJwtSigningKey(),
            //         Valid = Configuration.GetJwtTokenExpirationInMinutes(),
            //     };
            // });

            // Add the authorization layer.
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddFunderMapsPolicy();
            });
        }

        // Adds the core authentication service to the container.
        services.AddScoped<AspNetCore.Services.SignInService>();
        services.AddTransient<AspNetCore.Authentication.ISecurityTokenProvider, AspNetCore.Authentication.JwtBearerTokenProvider>();

        // NOTE: Register the HttpContextAccessor service to the container.
        //       The HttpContextAccessor exposes a singleton holding the
        //       HttpContext within a scoped resolver, or null outside the scope.
        //       Some components require the HttpContext and its features when the
        //       related service is being resolved within the scope.
        services.AddHttpContextAccessor();

        services.AddRazorPages();
        services.AddServerSideBlazor();
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
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAspAppContext();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
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

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAspAppContext();

        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}
