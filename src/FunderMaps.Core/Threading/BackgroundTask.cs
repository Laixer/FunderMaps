using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace FunderMaps.Core.Threading
{
    // FUTURE: Rename to BackgroundJob
    /// <summary>
    ///     Base class to background tasks.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        ///     Background context.
        /// </summary>
        protected BackgroundTaskContext TaskContext { get; private set; }

        /// <summary>
        ///     Gets the service provider to the task scope.
        /// </summary>
        protected IServiceProvider ServiceProvider => TaskContext.ServiceProvider;

        /// <summary>
        ///     The tasks cancellation token.
        /// </summary>
        protected CancellationToken CancellationToken => TaskContext.CancellationToken;

        /// <summary>
        ///     Represents a type used to perform logging.
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        ///     Setup the task.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        protected virtual async Task SetupTaskAsync(BackgroundTaskContext context)
        {
            TaskContext = context;

            // We allways want to yield for the async state machine.
            await Task.Yield();

            Logger = ServiceProvider.GetRequiredService<ILogger<BackgroundTask>>();
        }

        /// <summary>
        ///     Execute asynchronous operation.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual async Task ExecuteAsync(BackgroundTaskContext context)
        {
            await SetupTaskAsync(context);

            Execute(context);
        }

        /// <summary>
        ///     Execute synchronous operation.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual void Execute(BackgroundTaskContext context)
        {
            // NOTE: This is deliberately left empty for any derived class.
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
