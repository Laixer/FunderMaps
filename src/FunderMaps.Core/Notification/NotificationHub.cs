using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Process notification envelopes.
    /// </summary>
    internal class NotificationHub : AppServiceBase, INotifyService
    {
        private const string TaskName = "NOTIFICATION";

        private readonly BackgroundTaskScopedDispatcher _backgroundTaskDispatcher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NotificationHub(AppContext appContext, BackgroundTaskScopedDispatcher backgroundTaskDispatcher)
            => (AppContext, _backgroundTaskDispatcher) = (appContext, backgroundTaskDispatcher);

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        public async Task NotifyAsync(Envelope envelope)
            => await _backgroundTaskDispatcher.EnqueueTaskAsync(TaskName, envelope);
    }
}
