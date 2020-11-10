using System;
using System.Text.Json;
using System.Threading.Tasks;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Threading;

namespace FunderMaps.BatchNode.Jobs.Notification
{
    public abstract class NotificationTask : BackgroundTask
    {
        private const string TaskName = "notification";

        /// <summary>
        ///     Prepare the notification for the handler.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public override async Task ExecuteAsync(BackgroundTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var envelope = JsonSerializer.Deserialize<Envelope>(context.Value as string);
            if (envelope == null)
            {
                throw new ProtocolException();
            }

            await NotifyAsync(context, envelope);
        }

        /// <summary>
        ///     Method to check if a task can be handled by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
        {
            if (!(name.ToLowerInvariant() == TaskName && value is string))
            {
                return false;
            }

            var envelope = JsonSerializer.Deserialize<Envelope>(value as string);
            return envelope != null;
        }

        /// <summary>
        ///     Method to check if the envelope can be handeld by this notification handler.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public abstract bool CanHandle(Envelope envelope);

        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <param name="context">Notification context.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public abstract Task NotifyAsync(BackgroundTaskContext context, Envelope envelope);
    }
}
