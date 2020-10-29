using FunderMaps.Console.Exceptions;
using FunderMaps.Console.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Console.Services
{
    /// <summary>
    ///     Wrapper that handles our queue.
    /// </summary>
    public class QueueManager
    {
        private readonly Queue<BackgroundTaskSynchronous> QueueSynchronous = new Queue<BackgroundTaskSynchronous>();

        private readonly ConsoleOptions _options;
        private readonly ILogger<QueueManager> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueManager(IOptions<ConsoleOptions> options,
            ILogger<QueueManager> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Enqueue a background task.
        /// </summary>
        /// <param name="backgroundTask"></param>
        public void EnqueueTask(BackgroundTask backgroundTask)
        {
            if (backgroundTask == null)
            {
                throw new ArgumentNullException(nameof(backgroundTask));
            }

            switch (backgroundTask)
            {
                case BackgroundTaskSynchronous s:
                    FireSynchronousTask(s);
                    break;
                case BackgroundTaskAsynchronous a:
                    FireAsynchronousTask(a);
                    break;
                default:
                    throw new InvalidOperationException(nameof(BackgroundTask));
            }
        }

        /// <summary>
        ///     Attempts to enqueue synchronous work to our thread pool.
        /// </summary>
        /// <param name="backgroundTask">The task object to execute.</param>
        private void FireSynchronousTask(BackgroundTaskSynchronous backgroundTask) 
            => ThreadPool.QueueUserWorkItem(DoWork, Guid.NewGuid());

        // TODO Dummy function
        private void DoWork(object stateInfo) 
            => System.Console.WriteLine($"Do work called! {stateInfo.ToString()}");

        private void FireAsynchronousTask(BackgroundTaskAsynchronous backgroundTask)
        {
            // TODO Extact task.
            // TODO Fire to taskfactory.

            var task = new Task(() => System.Console.WriteLine("hi"));
            //TaskCreationOptions.
            //Task.Factory.StartNew()

            _logger.LogTrace($"Enqueued asynchronous background task {backgroundTask.Id}");
        }

        // TODO Not used atm
        private void TryEnqueueSynchronousTask(BackgroundTaskSynchronous backgroundTask)
        {
            if (QueueSynchronous.Count >= _options.MaxQueueSize)
            {
                throw new QueueFullException();
            }

            QueueSynchronous.Enqueue(backgroundTask);

            _logger.LogTrace($"Enqueued synchronous background task {backgroundTask.Id}");
        }
    }
}
