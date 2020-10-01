using System;
using FunderMaps.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions for configuring MVC using an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class MvcCoreMvcBuilderExtensions
    {
        /// <summary>
        ///     Adds the FunderMaps AspNetCore application part to the application parts.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddFunderMapsAssembly(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddApplicationPart(typeof(Constants).Assembly);

            return builder;
        }
    }
}