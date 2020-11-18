using System;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Task bucket represents tasks which will be run in the future.
    /// </summary>
    public record TaskBucket
    {
        /// <summary>
        ///     Type of task to run.
        /// </summary>
        public Type TaskType { get; init; }

        /// <summary>
        ///     Task runner context.
        /// </summary>
        public BackgroundTaskContext Context { get; init; }

        /// <summary>
        ///     The task id.
        /// </summary>
        public Guid TaskId => Context.Id;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TaskBucket()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TaskBucket(object value)
            => Context = new BackgroundTaskContext(taskId: Guid.NewGuid())
            {
                Value = value,
            };

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TaskBucket(object value, Type taskType)
            => (Context, TaskType) = (new BackgroundTaskContext(taskId: Guid.NewGuid())
            {
                Value = value,
            }, taskType);
    }
}
