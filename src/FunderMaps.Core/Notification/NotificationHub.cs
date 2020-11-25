using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Interfaces;
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

        private readonly IBatchService _batchService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NotificationHub(AppContext appContext, IBatchService batchService)
        {
            AppContext = appContext;
            _batchService = batchService;
        }

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        public Task DispatchNotifyAsync(Envelope envelope)
            => _batchService.EnqueueAsync(TaskName, envelope, AppContext.CancellationToken);

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="taskName">Name of the task to handle the job.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public Task DispatchNotifyAsync(string taskName, Envelope envelope)
            => _batchService.EnqueueAsync(taskName, envelope, AppContext.CancellationToken);            
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
