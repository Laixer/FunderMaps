using FunderMaps.AspNetCore.Extensions;
using FunderMaps.AspNetCore.HealthChecks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

// Register components from reference assemblies.
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsDataServices();
builder.Services.Configure<FunderMaps.Data.Providers.DbProviderOptions>(options =>
{
    options.ConnectionString = builder.Configuration["ConnectionStrings:FunderMapsConnection"];
});

// Add the authorization layer.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddFunderMapsPolicy();
});



// Adds the core authentication service to the container.
builder.Services.AddScoped<SignInService>();
builder.Services.AddTransient<ISecurityTokenProvider, JwtBearerTokenProvider>();

// NOTE: Register the HttpContextAccessor service to the container.
//       The HttpContextAccessor exposes a singleton holding the
//       HttpContext within a scoped resolver, or null outside the scope.
//       Some components require the HttpContext and its features when the
//       related service is being resolved within the scope.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
// builder.Services.AddHealthChecks().AddCheck<RepositoryHealthCheck>("data_health_check");

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
// app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
