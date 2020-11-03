using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types.BackgroundTasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Wrapper that handles our queue.
    /// </summary>
    /// <remarks>
    ///     This class modifies the threadpool max worker threads. Upon disposal
    ///     these changes will be undone. All will be set to its original values.
    /// </remarks>
    public class DispatchManager : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly int defaultWorkerThreads;
        private readonly int defaultCompletionPortThreads;

        private readonly IEnumerable<BackgroundTask> _backgroundTasks;
        private readonly BackgroundWorkOptions _options;
        private readonly ILogger<DispatchManager> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DispatchManager(
            IEnumerable<BackgroundTask> backgroundTasks,
            IOptions<BackgroundWorkOptions> options,
            ILogger<DispatchManager> logger)
        {
            _backgroundTasks = backgroundTasks ?? throw new ArgumentNullException(nameof(backgroundTasks));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ThreadPool.GetMaxThreads(out defaultWorkerThreads, out defaultCompletionPortThreads);
            ThreadPool.SetMaxThreads(_options.MaxWorkers, _options.MaxWorkers);
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
            if (value is null)
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
        ///     Generates a callback to execute a <paramref name="backgroundTask"/>
        ///     with some value object.
        /// </summary>
        /// <remarks>
        ///     This creates a new <see cref="BackgroundTaskContext"/>.
        /// </remarks>
        /// <param name="backgroundTask">The background task to execute.</param>
        /// <param name="value">The enqueued value object.</param>
        private WaitCallback BuildWaitCallback(BackgroundTask backgroundTask, object value)
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
                    _logger.LogDebug($"Starting background task {context.Id}");

                    await backgroundTask.ProcessAsync(context);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Exception in background task {context.Id}");
                }
                finally
                {
                    _logger.LogDebug($"Finished background task {context.Id}");
                }
            };
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
    }
}
