using FunderMaps.Core.Threading;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode
{
    // TODO Question -> In the protobuf file the service is called
    //                  EnqueueService, according to the GRPC naming
    //                  conventions. This is the name we would want
    //                  to give to this class, what to do with this?
    /// <summary>
    ///     Service used to enqueue items using GRPC onto our queue manager.
    /// </summary>
    public class ItemEnqueueService : EnqueueService.EnqueueServiceBase
    {
        private readonly DispatchManager _dispatchManager;
        private readonly ILogger<ItemEnqueueService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ItemEnqueueService(
            DispatchManager dispatchManager,
            ILogger<ItemEnqueueService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _dispatchManager = dispatchManager ?? throw new ArgumentNullException(nameof(dispatchManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        ///     Enqueue an item onto our queue manager.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="context">The call context.</param>
        /// <returns>Response object containing a task id.</returns>
        public override async Task<EnqueueResponse> EnqueueItem(EnqueueRequest request, ServerCallContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // TODO: This is a runtime err, maybe change ex
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentNullException(nameof(request.Name));
            }

            Guid taskid = await _dispatchManager.EnqueueTaskAsync(request.Name, request.Payload);

            return new EnqueueResponse
            {
                Protocol = new FunderMapsProtocol { },
                TaskId = taskid.ToString(),
            };
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
