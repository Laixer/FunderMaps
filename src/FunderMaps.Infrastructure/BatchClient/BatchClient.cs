using System.Text.Json;
using FunderMaps.BatchNode;
using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Infrastructure.BatchClient
{
    /// <summary>
    ///     Client connector to the batch node.
    /// </summary>
    internal class BatchClient : IBatchService // TODO: Inherit from AppServiceBase
    {
        private const string UserAgent = "FunderMaps.Infrastructure";

        private readonly ChannelFactory _channelFactory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BatchClient(ChannelFactory channelFactory)
            => _channelFactory = channelFactory;

        /// <summary>
        ///     Add task to batch queue.
        /// </summary>
        /// <param name="name">Name of task to run.</param>
        /// <param name="value">Task payload.</param>
        /// <param name="token">Canellation token.</param>
        public async Task<Guid> EnqueueAsync(string name, object value = null, CancellationToken token = default)
        {
            EnqueueRequest request = new()
            {
                Protocol = Protocol.BuildProtocol(UserAgent),
                Name = name,
                Payload = JsonSerializer.Serialize(value),
            };

            var client = new Batch.BatchClient(_channelFactory.RemoteChannel);
            EnqueueResponse response = await client.EnqueueAsync(request, null, null, token);
            return Guid.Parse(response.TaskId);
        }

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        public async Task TestService()
        {
            await EnqueueAsync("foobar");
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
