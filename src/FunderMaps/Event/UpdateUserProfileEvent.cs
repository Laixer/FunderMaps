﻿using FunderMaps.Models.Identity;

namespace FunderMaps.Event
{
    internal class UpdateUserProfileEvent : IUpdateUserProfileEvent
    {
        /// <summary>
        /// User account.
        /// </summary>
        public FunderMapsUser User { get; set; }
    }
}
