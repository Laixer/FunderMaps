using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    internal class NotificationHub : INotifyService // TODO: Inherit from app service
    {
        private readonly IEnumerable<INotifyHandler> _notifyHandlers;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NotificationHub(IEnumerable<INotifyHandler> notifyHandlers)
        {
            _notifyHandlers = notifyHandlers;
        }

        // TODO: CanellationToken
        private static async Task FireAndForget(INotifyHandler handler, Envelope envelope)
        {
            try
            {
                await Task.Yield();

                var context = new NotifyContext
                {
                    Envelope = envelope,
                    // Token = CancellationToken // TODO
                };

                await handler.Handle(context);

                // TODO: log status
            }
            catch (System.Exception)
            {
                // TODO: log ex

                throw;
            }
        }

        // TODO: Bulk
        /// <summary>
        ///     Notify by all means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containg the notification.</param>
        public void DispatchNotify(Envelope envelope)
        {
            foreach (var handler in _notifyHandlers)
            {
                Task.Run(() => FireAndForget(handler, envelope));
            }
        }

        // FUTURE: Replace with task scheduler.
        /// <summary>
        ///     Notify by all means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containg the notification.</param>
        /// <param name="notifyAt">Notify at future point in time.</param>
        public void DispatchNotify(Envelope envelope, DateTimeOffset notifyAt)
        {
            DispatchNotify(envelope);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
