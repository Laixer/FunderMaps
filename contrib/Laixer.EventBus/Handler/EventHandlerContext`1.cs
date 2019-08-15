namespace Laixer.EventBus.Handler
{
    public sealed class EventHandlerContext<TEvent> : EventHandlerContext
        where TEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the fired <see cref="TEvent"/> of the currently executing <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        public TEvent Event { get; set; }
    }
}
