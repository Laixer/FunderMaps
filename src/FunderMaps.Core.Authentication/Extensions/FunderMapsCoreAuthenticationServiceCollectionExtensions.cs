using FunderMaps.Core.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the core authentication service to the container.
        /// </summary>
        public static IServiceCollection AddFunderMapsCoreAuthentication(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<SignInService>();

            return services;
        }
    }
}
