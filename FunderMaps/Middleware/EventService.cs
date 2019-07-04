using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FunderMaps.Core.Event;

namespace FunderMaps.Middleware
{
    public class EventService : IEventService
    {
        //private readonly IList<IEventTriggerHandler> handlerList = new List<IEventTriggerHandler>();

        private IServiceProvider ServiceProvider { get; }

        public EventService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static IEnumerable<Type> GetDerivedInterfaces<TInterface>(object obj)
        {
            return obj.GetType()
                .GetInterfaces()
                .Where(type => type != typeof(TInterface) && typeof(TInterface).IsAssignableFrom(type));
        }

        /// <summary>
        /// Find all handlers which implement the event argument interface and call the handlers.
        /// </summary>
        /// <param name="argument">Event of type <see cref="ITriggerEvent"/>.</param>
        public virtual Task FireEventAsync(ITriggerEvent argument)
        {
            ICollection<Task> taskCollection = new List<Task>();

            foreach (Type type in GetDerivedInterfaces<ITriggerEvent>(argument))
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    Type genericHandlerType = typeof(IEventTriggerHandler<>).MakeGenericType(type);
                    var handlers = scope.ServiceProvider.GetServices(genericHandlerType);
                    foreach (var handler in handlers)
                    {
                        MethodInfo handleEventAsyncMethod = handler.GetType().GetMethod("HandleEventAsync");
                        if (handleEventAsyncMethod != null)
                        {
                            taskCollection.Add((Task)handleEventAsyncMethod.Invoke(handler, new object[] { argument }));
                        }
                    }
                }
            }

            return Task.WhenAll(taskCollection.ToArray());
        }
    }
}
