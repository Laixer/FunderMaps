using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extension methods for adding and replacing services to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Replaces all services in <see cref="IServiceCollection"/> with the same service type as descriptor
        ///     and adds descriptor to the collection. If the service is not found then a new descriptor is added
        ///      to the <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddOrReplace<TService>(this IServiceCollection services, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var serviceList = services.Where(s => s.ServiceType == typeof(TService));
            if (serviceList is not null && serviceList.Any())
            {
                // NOTE: The ToList() generates a complete list with known size. This circumvents
                //       in-place modification of the enumerable.
                foreach (var service in serviceList.ToList())
                {
                    services.Replace(new(typeof(TService), implementationFactory, service.Lifetime));
                }
            }
            else
            {
                services.Add(new(typeof(TService), implementationFactory, lifetime));
            }

            return services;
        }
    }
}
