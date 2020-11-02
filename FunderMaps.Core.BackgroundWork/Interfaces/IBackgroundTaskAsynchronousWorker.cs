using FunderMaps.Core.BackgroundWork.Types;
using System.Threading.Tasks;

namespace FunderMaps.Core.BackgroundWork.Interfaces
{
    /// <summary>
    ///     Contract for a background task worker for a specific task type.
    /// </summary>
    /// <typeparam name="TBackgroundTask">The type of background task.</typeparam>
    public interface IBackgroundTaskAsynchronousWorker<TBackgroundTask>
        where TBackgroundTask : BackgroundTask, new()
    {
        /// <summary>
        ///     Process an item asynchronously.
        /// </summary>
        /// <param name="item">The task to process.</param>
        Task ProcessAsync(TBackgroundTask item);
    }
}
