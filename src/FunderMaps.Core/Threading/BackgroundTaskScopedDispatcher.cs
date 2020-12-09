using FunderMaps.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Dispatch background task to manager. This dispatcher can only by called
    ///     from scoped service providers.
    /// </summary>
    public class BackgroundTaskScopedDispatcher
    {
        private readonly DispatchManager _dispatchManager;
        private readonly IEnumerable<BackgroundTask> _backgroundTaskList;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundTaskScopedDispatcher(IEnumerable<BackgroundTask> taskList, DispatchManager dispatchManager)
            => (_backgroundTaskList, _dispatchManager) = (taskList, dispatchManager ?? throw new ArgumentNullException(nameof(dispatchManager)));

        /// <summary>
        ///     Enqueues an object to process onto the queue.
        /// </summary>
        /// <remarks>
        ///     This <paramref name="value"/> is sent to each background task
        ///     implementation which is capable of processing the object. If
        ///     no implementation can process the object, nothing happens.
        /// </remarks>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload object.</param>
        /// <param name="delay">Optional task delay.</param>
        public virtual ValueTask<Guid> EnqueueTaskAsync(string name, object value = null, TimeSpan? delay = null)
        {
            TaskBucket bucket = new(value);

            var isQueued = false;
            foreach (var backgroundTask in _backgroundTaskList)
            {
                if (backgroundTask.CanHandle(name, value))
                {
                    _dispatchManager.QueueTaskItem(bucket with { TaskType = backgroundTask.GetType() }, delay);
                    isQueued = true;
                }
            }

            if (!isQueued)
            {
                throw new UnhandledTaskException();
            }

            return new(bucket.TaskId);
        }
    }
}
