using FunderMaps.Core.Extensions;
using FunderMaps.Core.ExternalServices;
using FunderMaps.Incident.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddFunderMapsAspNetCoreServices();
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsDataServices();

builder.Services.AddSingleton<PDOKLocationService>(); // TODO: Move to FunderMaps.Core

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

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

    app.UseExceptionHandler("/error");
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();

app.UseStaticFiles();

app.UseRouting();

app.UseAntiforgery();

app.UseAspAppContext();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = healthCheck => healthCheck.Tags.Contains("extern")
}).WithMetadata(new AllowAnonymousAttribute());

app.Run();
