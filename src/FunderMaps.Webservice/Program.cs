using FunderMaps.AspNetCore.Extensions;
using FunderMaps.AspNetCore.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(
    options => builder.Configuration.GetSection("JwtBearer").Bind(options));

// Add the authorization layer.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Register components from reference assemblies.
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsDataServices("FunderMapsConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks().AddCheck<RepositoryHealthCheck>("data_health_check");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    var forwardedOptions = new ForwardedHeadersOptions()
    {
        ForwardedHeaders = ForwardedHeaders.All,
    };

    forwardedOptions.KnownNetworks.Clear();
    forwardedOptions.KnownProxies.Clear();
    forwardedOptions.AllowedHosts.Clear();

    app.UseForwardedHeaders(forwardedOptions);
}

if (app.Environment.IsDevelopment())
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        Secure = CookieSecurePolicy.Always,
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/oops");
}

app.UsePathBase(new("/api"));
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAspAppContext();

app.MapControllers();
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
