namespace Laixer.EventBus.Handler
{
    /// <summary>
    /// Event handler context for the specified <typeparamref name="TEvent"/> type.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public sealed class EventHandlerContext<TEvent> : EventHandlerContext
        where TEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the fired <see cref="TEvent"/> of the currently executing <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        public TEvent Event { get; set; }
    }
}
