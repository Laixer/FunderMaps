using System;
using FunderMaps.Core.Event;
using FunderMaps.Models.Identity;

namespace FunderMaps.Event
{
    public class UpdateUserProfileEvent : IUpdateUserProfileEvent
    {
        /// <summary>
        /// User account.
        /// </summary>
        public FunderMapsUser User { get; set; }
    }
}
