﻿using System.Threading.Tasks;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Base class to background tasks.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        ///     Get the dispatch manager from context.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        /// <returns>Either <see cref="DispatchManager"/> or null.</returns>
        public DispatchManager GetDispatchManager(BackgroundTaskContext context)
            => context.DispatchManager.TryGetTarget(out DispatchManager manager) ? manager : null;

        /// <summary>
        ///     Execute asynchronous operation.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual async Task ExecuteAsync(BackgroundTaskContext context)
        {
            // We allways want to yield for the async state machine.
            await Task.Yield();

            Execute(context);
        }

        /// <summary>
        ///     Execute synchronous operation.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual void Execute(BackgroundTaskContext context)
        {
        }

        /// <summary>
        ///     Method to check if a task can handle a given object.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public abstract bool CanHandle(string name, object value);
    }
}
