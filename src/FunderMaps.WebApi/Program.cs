using FunderMaps.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFunderMapsAspNetCoreServices();
builder.Services.AddFunderMapsAspNetCoreAuth();
builder.Services.AddFunderMapsAspNetCoreControllers();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://api.pdok.nl");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
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
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseAspAppContext();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = healthCheck => healthCheck.Tags.Contains("extern")
}).WithMetadata(new AllowAnonymousAttribute());

app.Run();
