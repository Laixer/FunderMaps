using Laixer.EventBus;
using Laixer.EventBus.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering <see cref="EventBusService"/> in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="EventBusService"/> to the container, using the provided delegate to register events.
        /// </summary>
        /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// <see cref="EventBusService"/> instance in the <see cref="IServiceCollection"/>. It can be invoked
        /// multiple times in order to get access to the <see cref="IEventBusBuilder"/> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the <see cref="EventBusService"/> to.</param>
        /// <returns>An instance of <see cref="IEventBusBuilder"/> from which events can be registered.</returns>
        public static IEventBusBuilder AddEventBus(this IServiceCollection services)
        {
            services.AddOptions();

            services.TryAddSingleton<EventBusService, DefaultEventBusService>();

            return new EventBusBuilder(services);
        }
    }
}
