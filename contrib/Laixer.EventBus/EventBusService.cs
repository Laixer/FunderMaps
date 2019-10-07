using Laixer.EventBus.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Laixer.EventBus
{
    /// <summary>
    /// A service which can be used to check the status of <see cref="IEventHandler{TEvent}"/> instances 
    /// registered in the application.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The default implementation of <see cref="HealthCheckService"/> is registered in the dependency
    ///     injection container as a singleton service by calling 
    ///     <see cref="HealthCheckServiceCollectionExtensions.AddHealthChecks(IServiceCollection)"/>.
    /// </para>
    /// <para>
    ///     The <see cref="IHealthChecksBuilder"/> returned by 
    ///     <see cref="HealthCheckServiceCollectionExtensions.AddHealthChecks(IServiceCollection)"/>
    ///     provides a convenience API for registering health checks.
    /// </para>
    /// <para>
    ///     <see cref="IHealthCheck"/> implementations can be registered through extension methods provided by
    ///     <see cref="IHealthChecksBuilder"/>.
    /// </para>
    /// </remarks>
    public abstract class EventBusService
    {
        /// <summary>
        /// Runs all the health checks in the application and returns the aggregated status.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> which can be used to cancel the health checks.</param>
        public Task FireEventAsync(IEvent @event, CancellationToken cancellationToken = default)
            => FireEventAsync(predicate: null, @event, cancellationToken);

        /// <summary>
        /// Runs the provided health checks and returns the aggregated status
        /// </summary>
        /// <param name="predicate">
        /// A predicate that can be used to include health checks based on user-defined criteria.
        /// </param>
        /// <param name="event">Event to fire.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> which can be used to cancel the health checks.</param>
        public abstract Task FireEventAsync(
            Func<EventHandlerRegistration, bool> predicate,
            IEvent @event,
            CancellationToken cancellationToken = default);
    }
}
