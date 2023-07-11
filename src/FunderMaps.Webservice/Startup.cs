using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Data.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
namespace FunderMaps.Webservice;

/// <summary>
///     Application configuration.
/// </summary>
[Obsolete]
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
    ///     This method gets called by the runtime if no environment is set.
    /// </summary>
    /// <param name="services">See <see cref="IServiceCollection"/>.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        // services.AddAutoMapper(typeof(Startup));

        // var connectionString = Configuration.GetConnectionString("FunderMapsConnection");
        // services.AddFunderMapsDataServices();
        // services.Configure<DbProviderOptions>(options =>
        // {
        //     options.ConnectionString = connectionString;
        //     options.ApplicationName = "FunderMaps.Webservice";
        // });
    }

    /// <summary>
    ///     This method gets called by the runtime if environment is set to development.
    /// </summary>
    /// <param name="services">See <see cref="IServiceCollection"/>.</param>
    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
        // services.AddAutoMapper(typeof(Startup));

        // var connectionString = Configuration.GetConnectionString("FunderMapsConnection");
        // services.AddFunderMapsDataServices();
        // services.Configure<DbProviderOptions>(options =>
        // {
        //     options.ConnectionString = connectionString;
        //     options.ApplicationName = "FunderMaps.Webservice";
        // });

        // services.AddCors(options =>
        // {
        //     options.AddDefaultPolicy(policy =>
        //     {
        //         policy.AllowAnyHeader();
        //         policy.AllowAnyMethod();
        //         policy.AllowAnyOrigin();
        //     });
        // });
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
        // app.UseDeveloperExceptionPage();
        // app.UseCors();

        // app.UseExceptionHandler("/oops");

        // app.UsePathBase(new("/api"));
        // app.UseRouting();

        // app.UseAuthentication();
        // app.UseAuthorization();

        // app.UseAspAppContext();

        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        // });
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
        // app.UseForwardedHeaders(new()
        // {
        //     ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        // });

        // app.UseExceptionHandler("/oops");

        // app.UsePathBase(new("/api"));
        // app.UseRouting();

        // app.UseAuthentication();
        // app.UseAuthorization();

        // app.UseAspAppContext();

        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        //     endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
        // });
    }
}
