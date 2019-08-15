using Laixer.EventBus.Internal;

namespace Laixer.EventBus.Handler
{
    public abstract class EventHandlerContext
    {
        /// <summary>
        /// Gets or sets the <see cref="EventHandlerRegistration"/> of the currently executing <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        public EventHandlerRegistration Registration { get; set; }
    }
}
