using FunderMaps.Console.Types;
using System.Threading.Tasks;

namespace FunderMaps.Console.Interfaces
{
    /// <summary>
    ///     Contract for a background task worker for a specific task type.
    /// </summary>
    /// <typeparam name="TBackgroundTask">The type of background task.</typeparam>
    public interface IBackgroundTaskWorker<TBackgroundTask>
        where TBackgroundTask : BackgroundTask, new()
    {
        /// <summary>
        ///     Process an item asynchronously.
        /// </summary>
        /// <param name="item">The task to process.</param>
        Task ProcessAsync(TBackgroundTask item);
    }
}
