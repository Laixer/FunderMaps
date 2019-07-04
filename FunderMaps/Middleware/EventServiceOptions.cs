using System;
using System.Collections.Generic;
using FunderMaps.Core.Event;

namespace FunderMaps.Middleware
{
    public class EventServiceOptions
    {
        private readonly IList<Type> handlerTypeList = new List<Type>();

        public IEnumerable<Type> Handlers { get => handlerTypeList; }

        internal void Subscribe<TTriggerEvent, TInterface>()
            where TInterface : IEventTriggerHandler
        {
            handlerTypeList.Add(typeof(TInterface));
        }
    }
}
