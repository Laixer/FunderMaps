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
        private readonly BackgroundWorkOptions _options;
        private readonly ILogger<DispatchManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        private static ConcurrentQueue<TaskBucket> workerQueue = new ConcurrentQueue<TaskBucket>();
        private readonly List<TaskBucket> workerQueueDelay = new List<TaskBucket>();

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
            public BackgroundTaskContext Context { get; init; }

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
        public DispatchManager(IOptions<BackgroundWorkOptions> options, ILogger<DispatchManager> logger, IServiceProvider serviceProvider)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // NOTE: Add one more worker than configured. This will set the lower bound to 1
            //       and will disalign the workers to CPU core ratio.
            workerPoolHandle = new(_options.MaxWorkers + 1, (_options.MaxWorkers * 2) + 1);

            timer = new(obj => LaunchWorker(), null,
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
        /// <param name="value">The task payload object.</param>
        /// <param name="delay">Optional task delay.</param>
        public ValueTask<Guid> EnqueueTaskAsync(string name, object value, TimeSpan? delay = null)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TaskBucket bucket = new(_serviceProvider, value);

            var isQueued = false;
            foreach (var backgroundTask in _serviceProvider.GetServices<BackgroundTask>())
            {
                if (backgroundTask.CanHandle(name, value))
                {
                    bucket.BackgroundTask = backgroundTask;
                    QueueTaskItem(bucket, delay);
                    isQueued = true;
                }
            }

            if (!isQueued)
            {
                throw new UnhandledTaskException();
            }

            return new(bucket.TaskId);
        }

        /// <summary>
        ///     Enqueues an object to process onto the queue with provided task.
        /// </summary>
        /// <param name="value">The object to process.</param>
        /// <param name="delay">Optional task delay.</param>
        public ValueTask<Guid> EnqueueTaskAsync<TTask>(object value, TimeSpan? delay = null)
            where TTask : BackgroundTask
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TaskBucket bucket = new(_serviceProvider, value)
            {
                BackgroundTask = ActivatorUtilities.CreateInstance<TTask>(_serviceProvider),
            };

            QueueTaskItem(bucket, delay);
            return new(bucket.TaskId);
        }

        /// <summary>
        ///     Dispatch task item to the background, then return.
        /// </summary>
        /// <param name="taskBucket"><see cref="TaskBucket"/> to queue.</param>
        /// <param name="delay">Optional task delay.</param>
        protected virtual void QueueTaskItem(TaskBucket taskBucket, TimeSpan? delay = null)
        {
            if (taskBucket is null)
            {
                throw new ArgumentNullException(nameof(taskBucket));
            }

            if (delay.HasValue && delay != TimeSpan.Zero)
            {
                if (workerQueueDelay.Count > _options.MaxQueueSize)
                {
                    throw new QueueOverflowException();
                }

                taskBucket.Context.Delay = delay.Value;
                taskBucket.Context.QueuedAt = DateTime.Now;
                workerQueueDelay.Add(taskBucket);
            }
            else
            {
                if (workerQueue.Count > _options.MaxQueueSize)
                {
                    throw new QueueOverflowException();
                }

                taskBucket.Context.QueuedAt = DateTime.Now;
                workerQueue.Enqueue(taskBucket);
            }

            _logger.LogInformation($"Queue background task {taskBucket.TaskId}");

            LaunchWorker();
        }

        /// <summary>
        ///     Fire a worker, if possible, then return.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This operation should never block. The worker delegate is spawn on another
        ///         execution thread of execution which guarantees this thread is not blocked.
        ///     </para>
        ///     <para>
        ///         The task concurrency documentation does not recommend the usage of task workers
        ///         for synchronous CPU-bound work. However, this will not lead to manjor scalability
        ///         issues because we put a restriction on the concurrency upper-bound.
        ///     </para>
        /// </remarks>
        private void LaunchWorker()
        {
            // Run as long as there are items on the queue.
            async Task WorkerDelegate()
            {
                _logger.LogDebug("Allocating new worker");

                await workerPoolHandle.WaitAsync();

                try
                {
                    while (workerQueue.TryDequeue(out var taskBucket))
                    {
                        using var cts = new CancellationTokenSource(_options.TimeoutDelay == TimeSpan.Zero
                            ? TimeSpan.FromMinutes(15)
                            : _options.TimeoutDelay);

                        BackgroundTask backgroundTask = taskBucket.BackgroundTask;
                        BackgroundTaskContext context = taskBucket.Context;

                        using var ctsCombined = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, context.CancellationToken);

                        try
                        {
                            ctsCombined.Token.ThrowIfCancellationRequested();

                            _logger.LogInformation($"Starting background task {context.Id}");

                            context.StartedAt = DateTime.Now;
                            context.CancellationToken = ctsCombined.Token;
                            context.DispatchManager = new WeakReference<DispatchManager>(this);

                            await backgroundTask.ExecuteAsync(context);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, $"Exception in background task {context.Id}");

                            if (context.RetryCount == 0)
                            {
                                context.RetryCount++;
                                context.Delay = TimeSpan.FromMinutes(1);
                                QueueTaskItem(taskBucket);
                            }
                        }
                        finally
                        {
                            TimeSpan runningTime = DateTime.Now - context.StartedAt;

                            _logger.LogInformation($"Finished background task {context.Id} in {Math.Round(runningTime.TotalSeconds)}s");

                            await Task.Delay(250);
                        }
                    }
                }
                finally
                {
                    workerPoolHandle.Release();

                    _logger.LogDebug("Free worker");
                }
            }

            // Enqueue and remove anything overdue.
            workerQueueDelay.RemoveAll(b =>
            {
                if (DateTime.Now >= b.Context.QueuedAt + b.Context.Delay)
                {
                    workerQueue.Enqueue(b);
                    return true;
                }
                return false;
            });

            if (workerPoolHandle.CurrentCount > 0 && !workerQueue.IsEmpty)
            {
                Task.Run(() => WorkerDelegate());
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
