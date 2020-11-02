using System;
using FunderMaps.Core.Notification;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Notify service.
    /// </summary>
    public interface INotifyService
    {
        /// <summary>
        ///     Notify by all means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        void DispatchNotify(Envelope envelope);

        /// <summary>
        ///     Notify by all means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        /// <param name="notifyAt">Notify at future point in time.</param>
        void DispatchNotify(Envelope envelope, DateTimeOffset notifyAt);
    }
}
