using FunderMaps.AspNetCore.Components;
using Microsoft.AspNetCore.Builder;

namespace FunderMaps.AspNetCore.Extensions
{
    /// <summary>
    ///     Extension functionality for <see cref="AspAppContextMiddleware"/>.
    /// </summary>
    public static class AspAppContextMiddlewareExtensions
    {
        /// <summary>
        ///     Add <see cref="AspAppContextMiddleware"/> to the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseAspAppContext(this IApplicationBuilder builder)
            => builder.UseMiddleware<AspAppContextMiddleware>();
    }
}
