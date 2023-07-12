using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFunderMapsAspNetCoreServicesNew();

builder.Services.AddSingleton<PDOKLocationService>(); // TODO: Move to FunderMaps.Core

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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
    app.UseHttpsRedirection();

    app.UseExceptionHandler("/error");
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

app.UseRouting();

app.UseAspAppContext();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
