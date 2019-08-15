using System;
using Laixer.EventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class EventBusBuilder : IEventBusBuilder
    {
        public IServiceCollection Services { get; }

        public EventBusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IEventBusBuilder Add(EventHandlerRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            Services.Configure<EventBusServiceOptions>(options =>
            {
                options.Registrations.Add(registration);
            });

            return this;
        }
    }
}
