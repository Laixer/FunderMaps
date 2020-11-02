using FunderMaps.Core.BackgroundWork.Interfaces;
using FunderMaps.Core.BackgroundWork.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.BackgroundWork.Services
{
    // TODO How to give a cancellation token?
    /// <summary>
    ///     Wrapper that handles our queue.
    /// </summary>
    public class QueueManager : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        // Service Scope Factory oid, is er
        private readonly IServiceProvider _serviceProvider;
        private readonly ConsoleOptions _options;
        private readonly ILogger<QueueManager> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueManager(IServiceProvider serviceProvider,
            IOptions<ConsoleOptions> options,
            ILogger<QueueManager> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Enqueue a background task.
        /// </summary>
        /// <param name="backgroundTask"></param>
        public void EnqueueTask<TBackgroundTask>(TBackgroundTask backgroundTask)
            where TBackgroundTask : BackgroundTask, new()
        {
            if (backgroundTask == null)
            {
                throw new ArgumentNullException(nameof(backgroundTask));
            }

            if (backgroundTask.RunSynchronously)
            {
                FireSynchronousTask(backgroundTask);
            }
            else
            {
                FireAsynchronousTask(backgroundTask);
            }
        }

        /// <summary>
        ///     Attempts to enqueue synchronous work to our thread pool.
        /// </summary>
        /// <param name="backgroundTask">The task object to execute.</param>
        private void FireSynchronousTask<TBackgroundTask>(TBackgroundTask backgroundTask)
            where TBackgroundTask : BackgroundTask, new()
        {
            // TODO Will this work with dispose race conditions?
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetService<IBackgroundTaskSynchronousWorker<TBackgroundTask>>();
            if (service == null)
            {
                // TODO Custom exception
                throw new InvalidOperationException("Could not resolve service for background task type");
            }

            // TODO Max size?
            // TODO Correct way to handle sync work?
            // Enqueue using built in threadpool work items.
            try
            {
                ThreadPool.QueueUserWorkItem((object o) => service.Process(backgroundTask));
                _logger.LogTrace($"Enqueued synchronous background task {backgroundTask.Id}");
            }
            catch (NotSupportedException e)
            {
                _logger.LogError(e, $"Synchronous worker item {backgroundTask.Id} could not be enqueued");
            }
        }

        /// <summary>
        ///     Fires a new task using the <see cref="Task.Factory"/>.
        /// </summary>
        /// <param name="backgroundTask">The task object to execute.</param>
        private void FireAsynchronousTask<TBackgroundTask>(TBackgroundTask backgroundTask)
            where TBackgroundTask : BackgroundTask, new()
        {
            // TODO Will this work with dispose race conditions?
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetService<IBackgroundTaskAsynchronousWorker<TBackgroundTask>>();
            if (service == null)
            {
                // TODO Custom exception
                throw new InvalidOperationException("Could not resolve service for background task type");
            }

            // TODO What does ctoken here mean exactly
            // TODO TaskCreationOptions
            Task.Factory.StartNew(async () => await service.ProcessAsync(backgroundTask), cts.Token);

            _logger.LogTrace($"Enqueued asynchronous background task {backgroundTask.Id}");
        }

        // TODO DisposeAsync?
        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        public void Dispose() => cts.Dispose();
    }
}
