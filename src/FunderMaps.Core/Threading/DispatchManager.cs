using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
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
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly int defaultWorkerThreads;
        private readonly int defaultCompletionPortThreads;

        private readonly IEnumerable<BackgroundTask> _backgroundTasks;
        private readonly BackgroundWorkOptions _options;
        private readonly ILogger<DispatchManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DispatchManager(
            IEnumerable<BackgroundTask> backgroundTasks,
            IOptions<BackgroundWorkOptions> options,
            ILogger<DispatchManager> logger,
            IServiceProvider serviceProvider)
        {
            _backgroundTasks = backgroundTasks ?? throw new ArgumentNullException(nameof(backgroundTasks));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            ThreadPool.GetMaxThreads(out defaultWorkerThreads, out defaultCompletionPortThreads);
            ThreadPool.SetMaxThreads(_options.MaxWorkers, _options.MaxWorkers);
        }

        /// <summary>
        ///     Destroy instance.
        /// </summary>
        ~DispatchManager()
        {
            ThreadPool.SetMaxThreads(defaultWorkerThreads, defaultCompletionPortThreads);
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

            foreach (var backgroundTask in _backgroundTasks)
            {
                if (backgroundTask.CanHandle(value))
                {
                    QueueTaskItem(backgroundTask, value);
                }
            }
        }

        /// <summary>
        ///     Enqueues an object to process onto the queue with provided task.
        /// </summary>
        /// <param name="value">The object to process.</param>
        public void EnqueueTask<TTask>(object value)
            where TTask : BackgroundTask
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            QueueTaskItem(ActivatorUtilities.CreateInstance<TTask>(_serviceProvider), value);
        }

        /// <summary>
        ///     Dispatch task item to the background, then return.
        /// </summary>
        protected virtual void QueueTaskItem(BackgroundTask backgroundTask, object value)
        {
            if (ThreadPool.PendingWorkItemCount > _options.MaxQueueSize)
            {
                throw new QueueFullException();
            }

            // TODO: Rewrite {
            var appContext = _serviceProvider.GetRequiredService<AppContext>();
            cts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, appContext.CancellationToken);

            var context = new BackgroundTaskContext
            {
                CancellationToken = cts.Token,
                Value = value,
            };
            // }

            _logger.LogInformation($"Queue background task {context.Id}");

            ThreadPool.QueueUserWorkItem(BuildWaitCallback(backgroundTask, context));
        }

        /// <summary>
        ///     Generates a callback to execute a <paramref name="backgroundTask"/>
        ///     with some value object.
        /// </summary>
        /// <remarks>
        ///     This creates a new <see cref="BackgroundTaskContext"/>.
        /// </remarks>
        /// <param name="backgroundTask">Background task to execute.</param>
        /// <param name="context">Background task execution context.</param>
        private WaitCallback BuildWaitCallback(BackgroundTask backgroundTask, BackgroundTaskContext context)
        {
            return async (object o) =>
            {
                try
                {
                    _logger.LogInformation($"Starting background task {context.Id}");

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
            cts.Dispose();
        }
    }
}
