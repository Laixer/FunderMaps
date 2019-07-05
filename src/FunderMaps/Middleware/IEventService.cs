using FunderMaps.Core.Event;
using System.Threading.Tasks;

namespace FunderMaps.Middleware
{
    public interface IEventService
    {
        Task FireEventAsync(ITriggerEvent argument);
    }
}
