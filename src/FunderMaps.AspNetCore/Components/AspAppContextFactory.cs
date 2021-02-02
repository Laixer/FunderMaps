using System.Collections.Generic;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Components;
using FunderMaps.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.AspNetCore.Components
{
    /// <summary>
    ///     ASP.NET Core <see cref="Core.AppContext"/> factory.
    /// </summary>
    public sealed class AspAppContextFactory : AppContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AspAppContextFactory(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        /// <summary>
        ///     Create the <see cref="Core.AppContext"/> from the <see cref="HttpContext"/>.
        /// </summary>
        /// <remarks>
        ///     The HTTP context accessor is a singleton provided by the ASP.NET framework. The singleton
        ///     offers access to the <see cref="HttpContext"/> within the current scope. There does *not*
        ///     have to be an active scope, in which case the accessor returns null on the
        ///     <see cref="HttpContext"/> request. If the aforementioned HTTP context accessor is null then
        ///     we'll pass the action back to the base.
        /// </remarks>
        public override Core.AppContext Create()
        {
            if (_httpContextAccessor.HttpContext is not HttpContext httpContext)
            {
                return base.Create();
            }

            Dictionary<object, object> requestItems = new(httpContext.Items)
            {
                { "domain", httpContext.Request.Host.ToString() },
                { "remote-agent", httpContext.Request.Headers["User-Agent"].ToString() },
                { "remote-address", httpContext.Connection.RemoteIpAddress.ToString() },
            };

            if (PrincipalProvider.IsSignedIn(httpContext.User))
            {
                var (user, tenant) = PrincipalProvider.GetUserAndTenant<User, Organization>(httpContext.User);
                return new()
                {
                    CancellationToken = httpContext.RequestAborted,
                    Items = requestItems,
                    User = user,
                    Tenant = tenant,
                };
            }

            return new()
            {
                CancellationToken = httpContext.RequestAborted,
                Items = requestItems,
            };
        }
    }
}
