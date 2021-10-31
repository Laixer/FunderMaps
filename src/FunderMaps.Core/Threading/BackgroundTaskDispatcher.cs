namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Dispatch background task to manager. This dispatcher can be called
    ///     from all service providers.
    /// </summary>
    public class BackgroundTaskDispatcher
    {
        private readonly DispatchManager _dispatchManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundTaskDispatcher(DispatchManager dispatchManager)
            => _dispatchManager = dispatchManager ?? throw new ArgumentNullException(nameof(dispatchManager));

        /// <summary>
        ///     Enqueues an object to process onto the queue with provided task.
        /// </summary>
        /// <param name="value">The object to process.</param>
        /// <param name="delay">Optional task delay.</param>
        public ValueTask<Guid> EnqueueTaskAsync<TTask>(object value = null, TimeSpan? delay = null)
            where TTask : BackgroundTask
        {
            TaskBucket bucket = new(value, typeof(TTask));

            _dispatchManager.QueueTaskItem(bucket, delay);

            return new(bucket.TaskId);
        }
    }
}
