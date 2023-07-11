using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Services;
using FunderMaps.Data.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFunderMapsAspNetCoreServicesNew();
builder.Services.AddFunderMapsAspNetCoreAuth();
builder.Services.AddFunderMapsAspNetCoreControllers();

var connectionString = builder.Configuration.GetConnectionString("FunderMapsConnection");
builder.Services.AddFunderMapsDataServices();
builder.Services.Configure<DbProviderOptions>(options =>
{
    options.ConnectionString = connectionString;
    options.ApplicationName = "FunderMaps.Webservice";
});
builder.Services.Configure<MapboxOptions>(builder.Configuration.GetSection(MapboxOptions.Section)); // TODO: Move to FunderMaps.Core

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
    app.UseHttpsRedirection();

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

app.UseStaticFiles();

app.UsePathBase(new("/api"));
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAspAppContext();

app.MapControllers();
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
