using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.AspNetCore.Middleware
{
    /// <summary>
    ///     Create the <see cref="Core.AppContext"/> from the <see cref="HttpContext"/>.
    /// </summary>
    public class AspAppContextMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AspAppContextMiddleware(RequestDelegate next) => _next = next;

        /// <summary>
        ///     Invoke this middleware.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
        /// <param name="appContext">The <see cref="AppContext"/>.</param>
        public async Task InvokeAsync(HttpContext httpContext, Core.AppContext appContext)
        {
            appContext.CancellationToken = httpContext.RequestAborted;

            appContext.Items = new(httpContext.Items)
            {
                { "domain", httpContext.Request.Host.ToString() },
            };

            if (httpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                appContext.Items.Add("remote-agent", httpContext.Request.Headers["User-Agent"].ToString());
            }

            if (httpContext.Connection.RemoteIpAddress is not null)
            {
                appContext.Items.Add("remote-address", httpContext.Connection.RemoteIpAddress.ToString());
            }

            if (PrincipalProvider.IsSignedIn(httpContext.User))
            {
                var (user, tenant) = PrincipalProvider.GetUserAndTenant<User, Organization>(httpContext.User);
                appContext.User = user;
                appContext.Tenant = tenant;
            }

            await _next(httpContext);
        }
    }
}
