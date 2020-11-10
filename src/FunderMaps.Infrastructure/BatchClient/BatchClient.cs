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
    internal class BatchClient : Batch.BatchClient, IBatchService // TODO: Inherit from AppServiceBase
    {
        private const int protocolVersion = 0xa1;

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
            var request = new EnqueueRequest
            {
                Protocol = new FunderMapsProtocol
                {
                    Version = protocolVersion,
                    UserAgent = "FunderMaps.Infrastructure",
                },
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
