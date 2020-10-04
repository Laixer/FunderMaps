using FunderMaps.Core.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsCoreAuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddFunderMapsCoreAuthentication(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<AuthManager>();

            return services;
        }
    }
}
