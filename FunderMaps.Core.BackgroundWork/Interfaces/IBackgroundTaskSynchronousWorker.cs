using FunderMaps.Core.BackgroundWork.Types;

namespace FunderMaps.Core.BackgroundWork.Interfaces
{
    /// <summary>
    ///     Contract for a background task worker for a specific task type.
    /// </summary>
    /// <typeparam name="TBackgroundTask">The type of background task.</typeparam>
    public interface IBackgroundTaskSynchronousWorker<TBackgroundTask>
        where TBackgroundTask : BackgroundTask, new()
    {
        /// <summary>
        ///     Process an item synchronously.
        /// </summary>
        /// <param name="item">The task to process.</param>
        void Process(TBackgroundTask item);
    }
}
