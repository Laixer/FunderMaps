using System;
using System.Linq;
using FunderMaps.Core.Event;
using FunderMaps.Middleware;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventServiceExtensions
    {
        public static IServiceCollection AddEventService(this IServiceCollection services)
        {
            services.AddSingleton<IEventService, EventService>();

            return services;
        }
    }
}

