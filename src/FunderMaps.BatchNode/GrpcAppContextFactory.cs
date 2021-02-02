using FunderMaps.Core.Components;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Default AppContext factory.
    /// </summary>
    public sealed class GrpcAppContextFactory : AppContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public GrpcAppContextFactory(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        /// <summary>
        ///     Create the <see cref="Core.AppContext"/>.
        /// </summary>
        public override Core.AppContext Create()
        {
            return new();
        }
    }
}
