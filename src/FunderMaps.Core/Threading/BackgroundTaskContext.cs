using System;
using System.Threading;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Context for executing a background task.
    /// </summary>
    public class BackgroundTaskContext
    {
        /// <summary>
        ///     The cancellation token.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     The task id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        ///     The object to process, if any.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Time at which the job was queued.
        /// </summary>
        public DateTime QueuedAt { get; set; }

        /// <summary>
        ///     Time at which the job started execution.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        ///     Time at which the job finished.
        /// </summary>
        public DateTime FinishedAt { get; set; }

        /// <summary>
        ///     Delay the task.
        /// </summary>
        public TimeSpan Delay { get; set; }

        /// <summary>
        ///     Number of times the task was retried.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundTaskContext(Guid taskId)
        {
            Id = taskId;
        }
    }
}
