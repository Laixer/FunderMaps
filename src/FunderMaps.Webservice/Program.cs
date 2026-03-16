using FunderMaps.Core.Extensions;
using FunderMaps.Core.Middleware;
using FunderMaps.Data.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Kestrel: limit concurrent connections and set timeouts to prevent resource exhaustion.
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 200;
    options.Limits.MaxConcurrentUpgradedConnections = 50;
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddFunderMapsCoreServices();
builder.Services.AddFunderMapsDataServices();
builder.Services.AddFunderMapsAuthServices();

// Concurrency limiter: cap in-flight requests to prevent memory exhaustion under load.
// PgBouncer pool has 100 connections, so keep request concurrency aligned.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status503ServiceUnavailable;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: "global",
            factory: _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = 100,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 50,
            }));
});

builder.Services.AddControllers(options => options.Filters.Add(typeof(FunderMapsCoreExceptionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new FunderMaps.Core.Converters.DateOnlyJsonConverter()));

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

    forwardedOptions.KnownIPNetworks.Clear();
    forwardedOptions.KnownProxies.Clear();
    forwardedOptions.AllowedHosts.Clear();

    app.UseForwardedHeaders(forwardedOptions);

    app.UseHsts();

    app.UseExceptionHandler("/oops");
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors();
}

app.UseRouting();

app.UseRateLimiter();

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
