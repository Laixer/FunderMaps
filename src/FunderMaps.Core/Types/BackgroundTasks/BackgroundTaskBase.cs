using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Types.BackgroundTasks
{
    /// <summary>
    ///     Base class for executing a background task.
    /// </summary>
    public abstract class BackgroundTaskBase
    {
        /// <summary>
        ///     Do some asynchronous work.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual Task ProcessAsync(BackgroundTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // We allways want to yield for the async state machine.
            Task.Yield();

            Process(context);

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Do some synchronous work.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Process(BackgroundTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }

        /// <summary>
        ///     Method to check if a task can handle a given object.
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <returns>Boolean answer.</returns>
        public abstract bool CanHandle(object value);
    }
}
