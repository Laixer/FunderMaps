using FunderMaps.AspNetCore.Authorization;
using FunderMaps.AspNetCore.Extensions;
using FunderMaps.Core.Services;
using FunderMaps.Data.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthentication(config =>
// {
//     config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
// })
// .AddCookie(options =>
// {
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//     options.SlidingExpiration = true;
//     options.Cookie.Name = "LaixerAppAuth";
//     options.Cookie.MaxAge = TimeSpan.FromHours(10);
// })
// .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
//     options => builder.Configuration.GetSection("OpenIdConnect").Bind(options));

builder.Services.AddFunderMapsAspNetCoreServicesNew();

var connectionString = builder.Configuration.GetConnectionString("FunderMapsConnection");
builder.Services.AddFunderMapsDataServices();
builder.Services.Configure<DbProviderOptions>(options =>
{
    options.ConnectionString = connectionString;
    options.ApplicationName = "FunderMaps.RazorMaps";
});
builder.Services.Configure<MapboxOptions>(builder.Configuration.GetSection(MapboxOptions.Section)); // TODO: Move to FunderMaps.Core

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.Cookie.Name = "FunderMaps.Auth.Local";
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddFunderMapsPolicy();
});

builder.Services.AddControllers();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Account/Login");
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseAspAppContext();

app.MapControllers();
app.MapRazorPages();
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
