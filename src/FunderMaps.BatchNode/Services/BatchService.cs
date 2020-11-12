using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Threading;
using FunderMaps.Infrastructure.BatchClient;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Service used to enqueue items using GRPC onto our queue manager.
    /// </summary>
    public class BatchService : Batch.BatchBase
    {
        private const string UserAgent = "FunderMaps.BatchNode";

        private readonly DispatchManager _dispatchManager;
        private readonly ILogger<BatchService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BatchService(DispatchManager dispatchManager, ILogger<BatchService> logger)
        {
            _dispatchManager = dispatchManager ?? throw new ArgumentNullException(nameof(dispatchManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Enqueue an item onto our queue manager.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="context">The call context.</param>
        /// <returns>Response object containing a task id.</returns>
        public override async Task<EnqueueResponse> Enqueue(EnqueueRequest request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // TODO: This is a runtime err, maybe change ex
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                if (request.Protocol.Version != Protocol.protocolVersion)
                {
                    throw new ProtocolException("Protocol version mismatch");
                }

                Guid taskid = await _dispatchManager.EnqueueTaskAsync(request.Name, request.Payload);

                return new()
                {
                    Protocol = Protocol.BuildProtocol(UserAgent),
                    TaskId = taskid.ToString(),
                };
            }
            catch (FunderMapsCoreException e)
            {
                _logger.LogError(e, "Exception occred while processing the request");

                return new()
                {
                    Protocol = Protocol.BuildProtocol(UserAgent),
                };
            }
        }
    }
}
