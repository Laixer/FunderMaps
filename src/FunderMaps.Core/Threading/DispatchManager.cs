using FunderMaps.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly IEnumerable<BackgroundTask> _backgroundTasks;
        private readonly BackgroundWorkOptions _options;
        private readonly ILogger<DispatchManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentQueue<QueuePair> workerQueue = new ConcurrentQueue<QueuePair>();

        private SemaphoreSlim pool;

        class QueuePair
        {   
            public BackgroundTask BackgroundTask { get; set; }
            public BackgroundTaskContext Context { get; set; }
        }

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

            pool = new SemaphoreSlim(_options.MaxWorkers, _options.MaxWorkers * 2);

            new Timer(obj => LaunchWorker(), null, 2 * 60 * 1000, 60 * 1000);
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
            if (workerQueue.Count > _options.MaxQueueSize)
            {
                throw new QueueFullException();
            }

            // TODO: Rewrite {
            var taskId = Guid.NewGuid();
            var appContext = _serviceProvider.GetRequiredService<AppContext>();

            var context = new BackgroundTaskContext(taskId)
            {
                CancellationToken = appContext.CancellationToken,
                Value = value,
            };
            // }

            _logger.LogInformation($"Queue background task {context.Id}");

            workerQueue.Enqueue(new QueuePair
            {
                BackgroundTask = backgroundTask,
                Context = context,
            });

            LaunchWorker();
        }

        /// <summary>
        ///     Generates a callback to execute a <see cref="BackgroundTask"/>.
        /// </summary>
        /// <remarks>
        ///     This creates a new <see cref="BackgroundTaskContext"/>.
        /// </remarks>
        private void LaunchWorker()
        {
            async void Callback(object o)
            {
                await pool.WaitAsync();

                try
                {
                    while (workerQueue.TryDequeue(out var pair))
                    {
                        using var cts = new CancellationTokenSource(_options.TimeoutDelay);

                        BackgroundTask backgroundTask = pair.BackgroundTask;
                        BackgroundTaskContext context = pair.Context;

                        using var ctsCombined = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, context.CancellationToken);

                        try
                        {
                            context.CancellationToken.ThrowIfCancellationRequested();

                            _logger.LogInformation($"Starting background task {context.Id}");

                            context.CancellationToken = ctsCombined.Token;

                            await backgroundTask.ExecuteAsync(context);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, $"Exception in background task {context.Id}");
                        }
                        finally
                        {
                            _logger.LogDebug($"Finished background task {context.Id}");

                            await Task.Delay(250);
                        }
                    }
                }
                finally
                {
                    pool.Release();
                }
            }

            if (pool.CurrentCount > 0 && workerQueue.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(Callback);
            }
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        public void Dispose()
        {
            pool?.Dispose();
        }
    }
}
