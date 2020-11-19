using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Batch service.
    /// </summary>
    public interface IBatchService
    {
        /// <summary>
        ///     Add task to batch queue.
        /// </summary>
        /// <param name="name">Name of task to run.</param>
        /// <param name="value">Task payload.</param>
        /// <param name="token">Canellation token.</param>
        /// <returns>Task identifier if task was enqueued.</returns>
        Task<Guid> EnqueueAsync(string name, object value, CancellationToken token = default);

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        Task TestService();
    }
}
