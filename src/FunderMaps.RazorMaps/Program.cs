using FunderMaps.AspNetCore.Extensions;
using FunderMaps.AspNetCore.HealthChecks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

var k = new FunderMaps.AspNetCore.FunderMapsStartup();
k.Configure(builder.WebHost);

// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    // .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

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


// Register components from reference assemblies.
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsDataServices("FunderMapsConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks().AddCheck<RepositoryHealthCheck>("data_health_check");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    ForwardedHeadersOptions forwardedOptions = new()
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
    app.UseCookiePolicy(new()
    {
        Secure = CookieSecurePolicy.Always,
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
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
