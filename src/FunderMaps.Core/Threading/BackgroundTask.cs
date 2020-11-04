using System.Threading.Tasks;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Base class to background tasks.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        ///     Do some asynchronous work.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual async Task ExecuteAsync(BackgroundTaskContext context)
        {
            // We allways want to yield for the async state machine.
            await Task.Yield();

            Execute(context);
        }

        /// <summary>
        ///     Do some synchronous work.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual void Execute(BackgroundTaskContext context)
        {
        }

        /// <summary>
        ///     Method to check if a task can handle a given object.
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <returns>Boolean answer.</returns>
        public abstract bool CanHandle(object value);
    }
}
