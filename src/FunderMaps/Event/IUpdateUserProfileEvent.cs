using FunderMaps.Models.Identity;
using Laixer.EventBus;

namespace FunderMaps.Event
{
    internal interface IUpdateUserProfileEvent : IEvent
    {
        FunderMapsUser User { get; set; }
    }
}
