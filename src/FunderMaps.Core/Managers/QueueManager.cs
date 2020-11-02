using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.BackgroundTasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FunderMaps.Core.Managers
{
    /// <summary>
    ///     Wrapper that handles our queue.
    /// </summary>
    /// <remarks>
    ///     This class modifies the threadpool max worker threads. Upon disposal
    ///     these changes will be undone. All will be set to its original values.
    /// </remarks>
    public class QueueManager : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly int defaultWorkerThreads;
        private readonly int defaultCompletionPortThreads;

        private readonly IEnumerable<BackgroundTaskBase> _backgroundTasks;
        private readonly BackgroundWorkOptions _options;
        private readonly ILogger<QueueManager> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueManager(IEnumerable<BackgroundTaskBase> backgroundTasks,
            IOptions<BackgroundWorkOptions> options,
            ILogger<QueueManager> logger)
        {
            _backgroundTasks = backgroundTasks ?? throw new ArgumentNullException(nameof(backgroundTasks));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Setup threadpool
            // ONTHOUDEN
            // defaultWorkerThreads = 
            ThreadPool.SetMaxThreads((int)_options.MaxWorkers, (int)_options.MaxWorkers);
        }

        /// <summary>
        ///     Enqueues an object to process onto the queue.
        /// </summary>
        /// <remarks>
        ///     This <paramref name="value"/> is sent to each background task
        ///     implementation which is capable of processing the object. If
        ///     no implementation can process the object, nothing happens.
        /// </remarks>
        /// <param name="value">The object to process.</param>
        public void EnqueueTask(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (ThreadPool.PendingWorkItemCount > _options.MaxQueueSize)
            {
                throw new QueueFullException();
            }

            // TODO This can exceed the max queue count but I don't think that's a problem.
            foreach (var backgroundTask in _backgroundTasks)
            {
                if (backgroundTask.CanHandle(value))
                {
                    ThreadPool.QueueUserWorkItem(BuildWaitCallback(backgroundTask, value));
                }
            }
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        /// <remarks>
        ///     This undoes our <see cref="ThreadPool"/> modification.
        /// </remarks>
        public void Dispose()
        {
            ThreadPool.SetMaxThreads(defaultWorkerThreads, defaultCompletionPortThreads);

            cts.Dispose();
        }

        /// <summary>
        ///     Generates a callback to execute a <paramref name="backgroundTask"/>
        ///     with some value object.
        /// </summary>
        /// <remarks>
        ///     This creates a new <see cref="BackgroundTaskContext"/>.
        /// </remarks>
        /// <param name="backgroundTask">The background task to execute.</param>
        /// <param name="value">The enqueued value object.</param>
        private WaitCallback BuildWaitCallback(BackgroundTaskBase backgroundTask, object value)
        {
            var context = new BackgroundTaskContext
            {
                CancellationToken = cts.Token,
                Value = value,
            };

            return async (object o) =>
            {
                try
                {
                    _logger.LogTrace($"Starting background task {context.Id}");

                    await backgroundTask.ProcessAsync(context);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Exception in background task {context.Id}");
                }
                finally
                {
                    _logger.LogTrace($"Finished background task {context.Id}");
                }
            };
        }
    }
}
