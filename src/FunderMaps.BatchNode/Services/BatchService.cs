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

        private readonly BackgroundTaskScopedDispatcher _backgroundTaskDispatcher;
        private readonly ILogger<BatchService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BatchService(BackgroundTaskScopedDispatcher backgroundTaskDispatcher, ILogger<BatchService> logger)
        {
            _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Enqueue an item onto the queue and return with response.
        /// </summary>
        /// <param name="request">The client request.</param>
        /// <param name="context">The call context.</param>
        /// <returns>Response object containing a task id.</returns>
        public override async Task<EnqueueResponse> Enqueue(EnqueueRequest request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                // If the other side communicates a protocol which is incompatible with ours then
                // we won't be able to do anything. The request is halted and returns an error code.
                if (!Protocol.IsCompatible(request.Protocol))
                {
                    throw new ProtocolException("Protocol version mismatch");
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new ProtocolException("Task name is mandatory");
                }

                _logger.LogTrace("Submit task via dispatcher");

                Guid taskid = await _backgroundTaskDispatcher.EnqueueTaskAsync(request.Name, request.Payload);

                _logger.LogTrace("Dispatcher enqueued task with success");
                _logger.LogTrace($"Task ID {taskid}");

                return new()
                {
                    Protocol = Protocol.BuildProtocol(UserAgent),
                    TaskId = taskid.ToString(),
                };
            }
            catch (FunderMapsCoreException e)
            {
                _logger.LogError(e, "Exception occred while queueing the task");

                return new()
                {
                    Protocol = Protocol.BuildProtocol(UserAgent),
                };
            }
        }
    }
}
