using FunderMaps.Core.BackgroundWork.Interfaces;
using FunderMaps.Core.BackgroundWork.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.BackgroundWork.Services
{
    /// <summary>
    ///     Base class for executing a type of background task.
    /// </summary>
    /// <typeparam name="TBackgroundTask">The task type.</typeparam>
    public abstract class BackgroundTaskAsynchronousWorker<TBackgroundTask> : IBackgroundTaskAsynchronousWorker<TBackgroundTask>
        where TBackgroundTask : BackgroundTask, new()
    {
        protected readonly ILogger<BackgroundTaskAsynchronousWorker<TBackgroundTask>> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundTaskAsynchronousWorker(ILogger<BackgroundTaskAsynchronousWorker<TBackgroundTask>> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        ///     Process the task.
        /// </summary>
        /// <remarks>
        ///     This also logs and manages exceptions.
        /// </remarks>
        /// <param name="item">The item to process.</param>
        public async Task ProcessAsync(TBackgroundTask item)
        {
            try
            {
                _logger.LogTrace($"Starting background task {item.Id}");

                await DoWorkAsync(item);

                _logger.LogTrace($"Finished background task {item.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception in background task {item.Id}");
            }
        }

        /// <summary>
        ///     Does the actual task work.
        /// </summary>
        protected abstract Task DoWorkAsync(TBackgroundTask item);
    }
}
