using System;

namespace Laixer.EventBus.Internal
{
    /// <summary>
    /// Represents the registration information associated with an <see cref="IEventHandler{TEvent}"/> implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The health check registration is provided as a separate object so that application developers can customize
    ///     how health check implementations are configured.
    /// </para>
    /// <para>
    ///     The registration is provided to an <see cref="IEventHandler{TEvent}"/> implementation during execution through
    ///     <see cref="EventHandlerContext{TEvent}.Registration"/>. This allows a handler implementation to access named
    ///     options or perform other operations based on the registered name.
    /// </para>
    /// </remarks>
    public sealed class EventHandlerRegistration
    {
        /// <summary>
        /// Event interface.
        /// </summary>
        public Type EventInterfaceType { get; }

        /// <summary>
        /// Event handler implementation type
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public EventHandlerRegistration(Type eventInterfaceType, Type implementationType)
        {
            EventInterfaceType = eventInterfaceType;
            ImplementationType = implementationType;
        }

        /// <summary>
        /// Create a new <see cref="EventHandlerRegistration"/>.
        /// </summary>
        /// <typeparam name="TEventHandlerInterface">Event interface of the type <see cref="IEvent"/>.</typeparam>
        /// <typeparam name="TImplementation">Handler for the event interface.</typeparam>
        /// <returns>Instance of <see cref="EventHandlerRegistration"/>.</returns>
        public static EventHandlerRegistration Register<TEventHandlerInterface, TImplementation>()
            => new EventHandlerRegistration(typeof(TEventHandlerInterface), typeof(TImplementation));
    }
}
