using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     FooBar dummy job.
    /// </summary>
    /// <remarks>
    ///     This job is never used neither should it be used anywhere.
    ///     Take this job as the template to new jobs.
    /// </remarks>
    internal class FooBarJob : BackgroundTask
    {
        private const string TaskName = "FOOBAR";

        private readonly ILogger<FooBarJob> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FooBarJob(ILogger<FooBarJob> logger) => _logger = logger;

        /// <summary>
        ///     Execute asynchronous operation.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public override Task ExecuteAsync(BackgroundTaskContext context)
        {
            _logger.LogInformation("FooBar job is executing");

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Method to check if this job can handle the task.
        /// </summary>
        /// <remarks>
        ///     How the job decides to accept or ignore the task is implementation
        ///     specific.
        /// </remarks>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToUpperInvariant() == TaskName;
    }
}
