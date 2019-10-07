using Laixer.EventBus.Handler;
using System.Threading;
using System.Threading.Tasks;

namespace Laixer.EventBus
{
    /// <summary>
    /// Event handler interface.
    /// </summary>
    /// <typeparam name="TEvent">Event to handle.</typeparam>
    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : IEvent
    {
        /// <summary>
        /// Runs the event handler.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        Task HandleEventAsync(EventHandlerContext<TEvent> context, CancellationToken cancellationToken = default);
    }
}
