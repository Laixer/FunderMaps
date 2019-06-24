using Microsoft.AspNetCore.Builder;

namespace FunderMaps.Middleware
{
    /// <summary>
    /// Security middleware extensions.
    /// </summary>
    public static class SecurityMiddlewareExtensions
    {
        /// <summary>
        /// Enable enhanced security.
        /// </summary>
        public static IApplicationBuilder UseEnhancedSecurity(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityMiddleware>(new SecurityBuilder().AddDefaultSecurePolicy().Build());
        }

        /// <summary>
        /// Enable enhanced security with security builder.
        /// </summary>
        public static IApplicationBuilder UseEnhancedSecurity(this IApplicationBuilder app, SecurityBuilder builder)
        {
            return app.UseMiddleware<SecurityMiddleware>(builder.Build());
        }
    }
}
