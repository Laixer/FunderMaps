using FunderMaps.Core.Managers;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunderMaps.Grpc
{
    // TODO Question -> In the protobuf file the service is called
    //                  EnqueueService, according to the GRPC naming
    //                  conventions. This is the name we would want
    //                  to give to this class, what to do with this?
    /// <summary>
    ///     Service used to enqueue items using GRPC onto our 
    ///     queue manager.
    /// </summary>
    public class ItemEnqueueService : EnqueueService.EnqueueServiceBase
    {
        private readonly QueueManager _queueManager;
        private readonly ILogger<ItemEnqueueService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ItemEnqueueService(QueueManager queueManager,
            ILogger<ItemEnqueueService> logger)
        {
            _queueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));
            _logger = logger;
        }
        
        /// <summary>
        ///     Enqueue an item onto our queue manager.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="context">The call context.</param>
        /// <returns>Response object containing a task id.</returns>
        public override Task<EnqueueResponse> EnqueueItem(EnqueueRequest request, ServerCallContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Deserialize the request payload to some json object to enqueue.
            // TODO Do we really want this wildcard-behaviour?
            var item = JsonSerializer.Deserialize<object>(request.Payload);

            // TODO This doesn't return a task id yet
            _queueManager.EnqueueTask(item);

            // TODO Respond with message, call BuildProtocol()
            throw new NotImplementedException();
        }

        // TODO Move to some centralized class.
        /// <summary>
        ///     Build a fundermaps protocol object for responses.
        /// </summary>
        /// <param name="dateRequest">The request timestamp.</param>
        /// <returns>Created protocol object.</returns>
        private static FunderMapsProtocol BuildProtocol(DateTimeOffset dateRequest)
            => new FunderMapsProtocol
            {
                //DateRequest =
            };
    }
}
