using System;
using System.Collections.Generic;
using System.Text;

namespace Laixer.EventBus
{
    /// <summary>
    /// Represent the registration information associated with an <see cref="IEventHandler{TEvent}"/> implementation.
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

        public Type EventInterfaceType { get; set; }
        public Type ImplementationType { get; set; }

        public EventHandlerRegistration() { }

        public EventHandlerRegistration(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
