using FunderMaps.Core.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Default AppContext factory.
    /// </summary>
    public class GrpcAppContextFactory : AppContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache; // TODO: For now

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public GrpcAppContextFactory(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            => (_httpContextAccessor, _memoryCache) = (httpContextAccessor, memoryCache);

        /// <summary>
        ///     Create the <see cref="Core.AppContext"/>.
        /// </summary>
        public override Core.AppContext Create()
        {
            return new()
            {
                Cache = _memoryCache, // TODO: Remove
            };
        }
    }
}
