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
        private ConcurrentQueue<TaskBucket> workerQueue = new ConcurrentQueue<TaskBucket>();

        private SemaphoreSlim workerPoolHandle;
        private Timer timer;
        private bool disposedValue;

        /// <summary>
        ///     Task bucket represents tasks which will be run in the future.
        /// </summary>
        protected class TaskBucket
        {
            /// <summary>
            ///     Task to run.
            /// </summary>
            public BackgroundTask BackgroundTask { get; set; }

            /// <summary>
            ///     Task runner context.
            /// </summary>
            public BackgroundTaskContext Context { get; set; }

            /// <summary>
            ///     The task id.
            /// </summary>
            public Guid TaskId => Context.Id;

            // TODO: Rewrite
            /// <summary>
            ///     Create new instance.
            /// </summary>
            public TaskBucket(IServiceProvider _serviceProvider, object value)
            {
                var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var scopedContainer = scope.ServiceProvider;

                var appContext = scopedContainer.GetService<AppContext>();

                Context = new BackgroundTaskContext(taskId: Guid.NewGuid())
                {
                    CancellationToken = appContext.CancellationToken,
                    Value = value,
                };
            }
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

            workerPoolHandle = new SemaphoreSlim(_options.MaxWorkers, _options.MaxWorkers * 2);

            timer = new Timer(obj => LaunchWorker(), null,
                TimeSpan.FromMinutes(2),
                TimeSpan.FromMinutes(1));
        }

        /// <summary>
        ///     Enqueues an object to process onto the queue.
        /// </summary>
        /// <remarks>
        ///     This <paramref name="value"/> is sent to each background task
        ///     implementation which is capable of processing the object. If
        ///     no implementation can process the object, nothing happens.
        /// </remarks>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        public ValueTask<Guid> EnqueueTaskAsync(string name, object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bucket = new TaskBucket(_serviceProvider, value);

            foreach (var backgroundTask in _backgroundTasks)
            {
                if (backgroundTask.CanHandle(name, value))
                {
                    bucket.BackgroundTask = backgroundTask;
                    QueueTaskItem(bucket);
                }
            }

            return new ValueTask<Guid>(bucket.TaskId);
        }

        /// <summary>
        ///     Enqueues an object to process onto the queue with provided task.
        /// </summary>
        /// <param name="value">The object to process.</param>
        public ValueTask<Guid> EnqueueTaskAsync<TTask>(object value)
            where TTask : BackgroundTask
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bucket = new TaskBucket(_serviceProvider, value)
            {
                BackgroundTask = ActivatorUtilities.CreateInstance<TTask>(_serviceProvider),
            };

            QueueTaskItem(bucket);
            return new ValueTask<Guid>(bucket.TaskId);
        }

        /// <summary>
        ///     Dispatch task item to the background, then return.
        /// </summary>
        protected virtual void QueueTaskItem(TaskBucket taskBucket)
        {
            if (taskBucket is null)
            {
                throw new ArgumentNullException(nameof(taskBucket));
            }

            if (workerQueue.Count > _options.MaxQueueSize)
            {
                throw new QueueFullException();
            }

            _logger.LogInformation($"Queue background task {taskBucket.TaskId}");

            workerQueue.Enqueue(taskBucket);

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
                _logger.LogDebug("Allocating worker");

                await workerPoolHandle.WaitAsync();

                try
                {
                    while (workerQueue.TryDequeue(out var taskBucket))
                    {
                        using var cts = new CancellationTokenSource(_options.TimeoutDelay);

                        BackgroundTask backgroundTask = taskBucket.BackgroundTask;
                        BackgroundTaskContext context = taskBucket.Context;

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
                            _logger.LogInformation($"Finished background task {context.Id}");

                            await Task.Delay(250);
                        }
                    }
                }
                finally
                {
                    workerPoolHandle.Release();

                    _logger.LogDebug("Free up worker");
                }
            }

            if (workerPoolHandle.CurrentCount > 0 && workerQueue.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(Callback);
            }
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Dispose();
                    workerPoolHandle.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
