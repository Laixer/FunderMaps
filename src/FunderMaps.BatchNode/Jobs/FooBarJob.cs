using System.Threading.Tasks;
using FunderMaps.Core.Threading;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.BatchNode.Jobs
{
    /// <summary>
    ///     FooBar dummy job.
    /// </summary>
    internal class FooBarJob : BackgroundTask
    {
        private const string TaskName = "FOOBAR";

        private readonly ILogger<FooBarJob> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FooBarJob(ILogger<FooBarJob> logger)
            => _logger = logger;

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
        ///     Method to check if this task can handle a given object.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToUpperInvariant() == TaskName;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
