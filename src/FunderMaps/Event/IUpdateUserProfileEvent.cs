using FunderMaps.Core.Event;
using FunderMaps.Models.Identity;

namespace FunderMaps.Event
{
    internal interface IUpdateUserProfileEvent : ITriggerEvent
    {
        FunderMapsUser User { get; set; }
    }
}
