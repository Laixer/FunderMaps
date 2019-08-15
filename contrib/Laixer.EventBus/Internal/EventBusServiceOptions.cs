using System.Collections.Generic;

namespace Laixer.EventBus.Internal
{
    /// <summary>
    /// Options for the default implementation of <see cref="EventBusService"/>
    /// </summary>
    public sealed class EventBusServiceOptions
    {
        /// <summary>
        /// Gets the event registrations.
        /// </summary>
        public ICollection<EventHandlerRegistration> Registrations { get; } = new List<EventHandlerRegistration>();
    }
}
