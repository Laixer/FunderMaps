using System.Threading.Tasks;

namespace FunderMaps.Core.Event
{
    public interface IEventTriggerHandler
    {
    }

    public interface IEventTriggerHandler<T> : IEventTriggerHandler
        where T : ITriggerEvent
    {
        Task HandleEventAsync(T triggerEvent);
    }
}
