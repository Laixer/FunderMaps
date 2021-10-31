using FunderMaps.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions for configuring MVC using an <see cref="IMvcBuilder"/>.
    /// </summary>
    internal static class MvcCoreMvcBuilderExtensions
    {
        /// <summary>
        ///     Adds the FunderMaps AspNetCore application part to the MVC application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddFunderMapsAssembly(this IMvcBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddApplicationPart(typeof(Constants).Assembly);

            return builder;
        }
    }
}
