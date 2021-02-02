using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Dummy batch service.
    /// </summary>
    internal class NullBatchService : IBatchService
    {
        /// <summary>
        ///     Add task to batch queue.
        /// </summary>
        /// <param name="name">Name of task to run.</param>
        /// <param name="value">Task payload.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task identifier if task was enqueued.</returns>
        public Task<Guid> EnqueueAsync(string name, object value, CancellationToken token = default)
            => Task.FromResult(Guid.NewGuid());

        /// <summary>
        ///     Batch service processing status.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public Task<DispatchManagerStatus> StatusAsync(CancellationToken token = default)
            => Task.FromResult(new DispatchManagerStatus());

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        public Task HealthCheck() => Task.CompletedTask;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
