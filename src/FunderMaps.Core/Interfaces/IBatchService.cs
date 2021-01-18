using System;
using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;

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
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task identifier if task was enqueued.</returns>
        Task<Guid> EnqueueAsync(string name, object value, CancellationToken token = default);

        /// <summary>
        ///     Batch service processing status.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        Task<DispatchManagerStatus> StatusAsync(CancellationToken token = default);

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        Task TestService();
    }
}
