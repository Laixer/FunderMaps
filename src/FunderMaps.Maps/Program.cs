using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "http://localhost:8080/realms/master";
    options.CallbackPath = "/redirect";

    options.ClientId = "fundermaps-maps";
    options.ClientSecret = "6AOtIBlPIMXHPttZnlUiwrOpKMXziPw6";
    options.ResponseType = "code";

    options.RequireHttpsMetadata = false;

    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;
});
builder.Services.AddAuthorization();

// Register components from reference assemblies.
builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsInfrastructureServices();
builder.Services.AddFunderMapsDataServices("FunderMapsConnection");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseForwardedHeaders(new()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
});

if (app.Environment.IsDevelopment())
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        MinimumSameSitePolicy = SameSiteMode.Lax
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
