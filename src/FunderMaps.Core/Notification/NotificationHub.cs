using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Process notification envelopes.
    /// </summary>
    internal class NotificationHub : AppServiceBase, INotifyService
    {
        private const string TaskName = "notification";

        private readonly BackgroundTaskScopedDispatcher _backgroundTaskDispatcher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NotificationHub(AppContext appContext, BackgroundTaskScopedDispatcher backgroundTaskDispatcher)
        {
            AppContext = appContext;
            _backgroundTaskDispatcher = backgroundTaskDispatcher;
        }

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        public async Task DispatchNotifyAsync(Envelope envelope)
            => await _backgroundTaskDispatcher.EnqueueTaskAsync(TaskName, envelope);

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="taskName">Name of the task to handle the job.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public async Task DispatchNotifyAsync(string taskName, Envelope envelope)
            => await _backgroundTaskDispatcher.EnqueueTaskAsync(taskName, envelope);
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
