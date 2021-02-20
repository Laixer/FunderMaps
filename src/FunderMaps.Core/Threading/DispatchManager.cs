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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private static List<TaskBucket> workerQueueDelay = new();
        private static ConcurrentQueue<TaskBucket> workerQueue = new();

        private SemaphoreSlim workerPoolHandle;
        private Timer timer;
        private bool disposedValue;

        /// <summary>
        ///     Execution statistics.
        /// </summary>
        public DispatchManagerStatus Status { get; } = new();

        /// <summary>
        ///     Number of jobs in delay queue.
        /// </summary>
        public int WorkerQueueDelaySize => workerQueueDelay.Count;

        /// <summary>
        ///     Number of jobs in queue.
        /// </summary>
        public int WorkerQueueSize => workerQueue.Count;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DispatchManager(IOptions<BackgroundWorkOptions> options, ILogger<DispatchManager> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

            // NOTE: Add one more worker than configured. This will set the lower bound to 1
            //       and will misalign the number of workers-to-CPU-core ratio. Doing so will
            //       always keep a single core free for other processes.
            workerPoolHandle = new(_options.MaxWorkers, (_options.MaxWorkers * 2));

            timer = new(obj => LaunchWorker(), null,
                TimeSpan.FromMinutes(2),
                TimeSpan.FromMinutes(1));
        }

        /// <summary>
        ///     Dispatch task item to the background, then return.
        /// </summary>
        /// <param name="taskBucket"><see cref="TaskBucket"/> to queue.</param>
        /// <param name="delay">Optional task delay.</param>
        public virtual void QueueTaskItem(TaskBucket taskBucket, TimeSpan? delay = null)
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

                workerQueueDelay.Add(taskBucket with
                {
                    Context = taskBucket.Context with
                    {
                        Delay = delay.Value,
                        QueuedAt = DateTime.Now
                    }
                });
            }
            else
            {
                if (workerQueue.Count > _options.MaxQueueSize)
                {
                    throw new QueueOverflowException();
                }

                workerQueue.Enqueue(taskBucket with
                {
                    Context = taskBucket.Context with
                    {
                        QueuedAt = DateTime.Now
                    }
                });
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
            const int sleepAfterEachJobInMs = 250;

            // Run as long as there are items on the queue.
            async Task WorkerDelegate()
            {
                _logger.LogDebug("Allocating new worker");

                await workerPoolHandle.WaitAsync();

                try
                {
                    while (workerQueue.TryDequeue(out var taskBucket))
                    {
                        // Setup scope per task. We know for a fact that at there does not exist
                        // an active service provider from this. The scope is valid until the end
                        // of the tasks lifespan. Tasks can resolve any service within their scope.
                        using var serviceScope = _serviceScopeFactory.CreateScope();

                        using CancellationTokenSource cts = new(_options.TimeoutDelay == TimeSpan.Zero
                            ? TimeSpan.FromMinutes(15)
                            : _options.TimeoutDelay);

                        var backgroundTask = serviceScope.ServiceProvider.GetRequiredService(taskBucket.TaskType) as BackgroundTask;
                        BackgroundTaskContext context = taskBucket.Context;

                        try
                        {
                            _logger.LogInformation($"Starting background task {context.Id}");

                            context.ServiceProvider = serviceScope.ServiceProvider;
                            context.StartedAt = DateTime.Now;
                            context.CancellationToken = cts.Token;

                            await backgroundTask.ExecuteAsync(context);

                            Status.JobsSucceeded++;
                        }
                        catch (Exception e) // FUTURE: Check a specific exception
                        {
                            _logger.LogError($"background task {context.Id} failed");
                            _logger.LogDebug(e, $"Exception in background task {context.Id}");

                            if (context.RetryCount == 0)
                            {
                                context.RetryCount++;
                                QueueTaskItem(taskBucket, TimeSpan.FromMinutes(5));
                            }
                            else
                            {
                                Status.JobsFailed++;
                            }
                        }
                        finally
                        {
                            TimeSpan runningTime = DateTime.Now - context.StartedAt;

                            _logger.LogInformation($"Finished background task {context.Id} in {Math.Round(runningTime.TotalSeconds)}s");

                            await Task.Delay(sleepAfterEachJobInMs);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Exception when creating task");
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
