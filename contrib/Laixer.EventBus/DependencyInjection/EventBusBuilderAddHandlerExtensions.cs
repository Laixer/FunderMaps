using System;
using Laixer.EventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides basic extension methods for registering <see cref="IEventHandler"/> instances in an <see cref="IEventBusBuilder"/>.
    /// </summary>
    public static class EventBusBuilderAddHandlerExtensions
    {
        /// <summary>
        /// Register event handler for the event interface.
        /// </summary>
        /// <typeparam name="TEventInterface">Event interface of the type <see cref="IEvent"/>.</typeparam>
        /// <typeparam name="TImplementation">Handler for the event interface.</typeparam>
        /// <param name="builder">Instance of <see cref="IEventBusBuilder"/>.</param>
        /// <param name="name">Event handler name</param>
        /// <returns>Instance of <see cref="IEventBusBuilder"/>.</returns>
        public static IEventBusBuilder AddHandler<TEventInterface, TImplementation>(this IEventBusBuilder builder, string name)
            where TEventInterface : IEvent
            where TImplementation : class, IEventHandler<TEventInterface>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return builder.Add(new EventHandlerRegistration(name)
            {
                EventInterfaceType = typeof(TEventInterface),
                ImplementationType = typeof(TImplementation),
            });
        }

        /// <summary>
        /// Register event handler for the event interface.
        /// </summary>
        /// <typeparam name="TEventInterface">Event interface of the type <see cref="IEvent"/>.</typeparam>
        /// <typeparam name="TImplementation">Handler for the event interface.</typeparam>
        /// <param name="builder">Instance of <see cref="IEventBusBuilder"/>.</param>
        /// <returns>Instance of <see cref="IEventBusBuilder"/>.</returns>
        public static IEventBusBuilder AddHandler<TEventInterface, TImplementation>(this IEventBusBuilder builder)
            where TEventInterface : IEvent
            where TImplementation : class, IEventHandler<TEventInterface>, new()
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Add(new EventHandlerRegistration
            {
                EventInterfaceType = typeof(TEventInterface),
                ImplementationType = typeof(TImplementation),
            });
        }
    }
}
