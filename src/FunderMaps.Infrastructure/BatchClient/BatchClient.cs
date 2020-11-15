using System.Text.Json;
using FunderMaps.BatchNode;
using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;
using System.Threading;

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
        public async Task EnqueueAsync(string name, object value, CancellationToken token = default)
        {
            EnqueueRequest request = new()
            {
                Protocol = Protocol.BuildProtocol(UserAgent),
                Name = name,
                Payload = JsonSerializer.Serialize(value),
            };

            var client = new Batch.BatchClient(_channelFactory.RemoteChannel);
            await client.EnqueueAsync(request, null, null, token);
        }

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        public Task TestService()
        {
            throw new System.NotImplementedException();
        }
    }
}
