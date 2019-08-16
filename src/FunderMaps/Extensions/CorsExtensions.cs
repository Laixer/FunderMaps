using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.Extensions
{
    /// <summary>
    /// CORS extensions.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Set the default CORS policy for this application.
        /// </summary>
        /// <param name="services">See <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">See <see cref="IConfiguration"/>.</param>
        /// <returns>See <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var policy = new CorsPolicyBuilder()
                .WithOrigins("https://localhost:8080", "http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();

            var domain = configuration.GetDomainHost();
            if (domain != null)
            {
                policy.WithOrigins($"https://{domain}");
            }

            services.AddCors(options => options.AddPolicy("CORSDeveloperPolicy", policy.Build()));

            return services;
        }
    }
}
