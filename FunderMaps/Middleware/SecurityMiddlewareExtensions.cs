using System;
using Microsoft.AspNetCore.Builder;

namespace FunderMaps.Middleware
{
    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseEnhancedSecurity(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityMiddleware>(new SecurityBuilder().AddDefaultSecurePolicy().Build());
        }

        public static IApplicationBuilder UseEnhancedSecurity(this IApplicationBuilder app, SecurityBuilder builder)
        {
            return app.UseMiddleware<SecurityMiddleware>(builder.Build());
        }
    }
}
