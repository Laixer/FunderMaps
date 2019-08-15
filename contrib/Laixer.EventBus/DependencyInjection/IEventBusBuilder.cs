using Laixer.EventBus.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// A builder used to register the event bus.
    /// </summary>
    public interface IEventBusBuilder
    {
        /// <summary>
        /// Adds a <see cref="HealthCheckRegistration"/> for a health check.
        /// </summary>
        /// <param name="registration">The <see cref="HealthCheckRegistration"/>.</param>
        IEventBusBuilder Add(EventHandlerRegistration registration);

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> into which <see cref="IHealthCheck"/> instances should be registered.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
