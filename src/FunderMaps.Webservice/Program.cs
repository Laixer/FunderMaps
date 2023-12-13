using FunderMaps.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFunderMapsAspNetCoreServices();
builder.Services.AddFunderMapsAspNetCoreAuth();
builder.Services.AddFunderMapsAspNetCoreControllers();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCorsAllowAny();
}

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

    app.UseHsts();

    app.UseExceptionHandler("/oops");
}

if (app.Environment.IsDevelopment())
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        Secure = CookieSecurePolicy.Always,
    });

    app.UseDeveloperExceptionPage();
    app.UseCors();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAspAppContext();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = healthCheck => healthCheck.Tags.Contains("extern")
}).WithMetadata(new AllowAnonymousAttribute());

app.Run();

public partial class Program { }
