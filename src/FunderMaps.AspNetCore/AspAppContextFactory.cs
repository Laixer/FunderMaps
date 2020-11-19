using FunderMaps.Core.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Default AppContext factory.
    /// </summary>
    public class AspAppContextFactory : AppContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache; // TODO: For now

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AspAppContextFactory(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            => (_httpContextAccessor, _memoryCache) = (httpContextAccessor, memoryCache);

        /// <summary>
        ///     Create the <see cref="Core.AppContext"/> from the <see cref="HttpContext"/>.
        /// </summary>
        /// <remarks>
        ///     The HTTP context accessor is a singleton provided by the ASP.NET framework. The singleton
        ///     offers access to the <see cref="HttpContext"/> within the current scope. There does not
        ///     have to be an active scope, in which case the accessor returns null on the
        ///     <see cref="HttpContext"/> request. If the aforementioned HTTP context accessor is null then
        ///     we'll return an empty <see cref="Core.AppContext"/>.
        /// </remarks>
        public override Core.AppContext Create()
        {
            if (_httpContextAccessor.HttpContext is not HttpContext httpContext)
            {
                return new();
            }

            return new()
            {
                CancellationToken = httpContext.RequestAborted,
                Items = new(httpContext.Items),
                Cache = _memoryCache, // TODO: Remove
                User = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item1 : null,
                Tenant = Core.Authentication.PrincipalProvider.IsSignedIn(httpContext.User) ? Core.Authentication.PrincipalProvider.GetUserAndTenant<Core.Entities.User, Core.Entities.Organization>(httpContext.User).Item2 : null,
            };
        }
    }
}
