using System.Collections.Generic;

namespace Laixer.EventBus.Internal
{
    /// <summary>
    /// Options for the default implementation of <see cref="EventBusService"/>
    /// </summary>
    public sealed class EventBusServiceOptions
    {
        /// <summary>
        /// List of regstered events.
        /// </summary>
        public ICollection<EventHandlerRegistration> Registrations { get; } = new List<EventHandlerRegistration>();
    }
}
