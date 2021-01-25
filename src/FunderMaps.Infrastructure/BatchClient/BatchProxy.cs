using System.Text.Json;
using FunderMaps.BatchNode;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Threading;
using System.Threading.Tasks;
using System.Threading;
using System;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Infrastructure.BatchClient
{
    /// <summary>
    ///     Client connector to the batch node.
    /// </summary>
    internal class BatchProxy : IBatchService
    {
        private const string UserAgent = "FunderMaps.Infrastructure";

        private readonly ChannelFactory _channelFactory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BatchProxy(ChannelFactory channelFactory)
            => _channelFactory = channelFactory;

        /// <summary>
        ///     Add task to batch queue.
        /// </summary>
        /// <param name="name">Name of task to run.</param>
        /// <param name="value">Task payload.</param>
        /// <param name="token">Cancellation token.</param>
        public async Task<Guid> EnqueueAsync(string name, object value = null, CancellationToken token = default)
        {
            EnqueueRequest request = new()
            {
                Protocol = Protocol.BuildProtocol(UserAgent),
                Name = name,
                Payload = JsonSerializer.Serialize(value),
            };

            Batch.BatchClient client = new(_channelFactory.RemoteChannel);
            EnqueueResponse response = await client.EnqueueAsync(request, null, null, token);

            return Guid.Parse(response.TaskId);
        }

        /// <summary>
        ///     Batch service processing status.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public async Task<DispatchManagerStatus> StatusAsync(CancellationToken token = default)
        {
            Batch.BatchClient client = new(_channelFactory.RemoteChannel);
            StatusResponse response = await client.StatusAsync(new(), null, null, token);

            return new()
            {
                JobsSucceeded = response.JobsSucceeded,
                JobsFailed = response.JobsFailed,
            };
        }

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        public Task HealthCheck() => StatusAsync();
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
