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

            services.AddFunderMapsCoreAuthentication(setupAction: null);

            return services;
        }

        public static IServiceCollection AddFunderMapsCoreAuthentication(this IServiceCollection services, Action<AuthenticationOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // TODO: AddScoped -> Transient?s

            services.AddScoped<AuthManager>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }
    }
}
