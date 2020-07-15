﻿using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Notification service.
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <summary>
        ///     Notify by email.
        /// </summary>
        /// <param name="address">Array of recipients.</param>
        public ValueTask NotifyByEmailAsync(string[] address)
        {
            return new ValueTask();
        }
    }
}
