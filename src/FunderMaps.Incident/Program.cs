using FunderMaps.AspNetCore.Extensions;
using FunderMaps.AspNetCore.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Register components from reference assemblies.
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsInfrastructureServices();
builder.Services.AddFunderMapsDataServices("FunderMapsConnection");

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
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

app.UseStaticFiles();

app.UseRouting();

app.UseAspAppContext();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
