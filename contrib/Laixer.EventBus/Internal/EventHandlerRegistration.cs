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
        private string _name;

        /// <summary>
        /// Gets or sets the handler name.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Event interface.
        /// </summary>
        public Type EventInterfaceType { get; set; }

        /// <summary>
        /// Event handler implementation type
        /// </summary>
        public Type ImplementationType { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public EventHandlerRegistration() { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="name">Name of the event handler.</param>
        public EventHandlerRegistration(string name)
            => _name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
