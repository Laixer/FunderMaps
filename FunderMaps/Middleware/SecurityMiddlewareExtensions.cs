using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace FunderMaps.Middleware
{
    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityBuilder builder)
        {
            SecurityPolicy policy = builder.Build();
            return app.UseMiddleware<SecurityMiddleware>(policy);
        }
    }
}
