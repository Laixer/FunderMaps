using System;
using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System.Linq;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Schedule tasks with interval.
    /// </summary>
    public class TimedHostedService : IHostedService, IDisposable
    {
        private const string ConfigurationScheduleSection = "Scheduler";

        private readonly IConfiguration _configuration;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private IServiceProvider _servicesProvider;

        private SemaphoreSlim signal = new(1);
        private ScheduleTask[] scheduleTasks;

        /// <summary>
        ///     Schedule task options.
        /// </summary>
        public record ScheduleTask
        {
            /// <summary>
            ///     Task to execute.
            /// </summary>
            public string TaskName { get; init; }

            /// <summary>
            ///     Task value.
            /// </summary>
            public string Value { get; init; }

            /// <summary>
            ///     Task interval.
            /// </summary>
            public string Interval { get; init; }
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TimedHostedService(IConfiguration configuration, IServiceProvider services, ILogger<TimedHostedService> logger)
        {
            _configuration = configuration;
            _servicesProvider = services;
            _logger = logger;
        }

        /// <summary>
        ///     Triggered when the application host is ready to start the service.
        /// </summary>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new(Worker, null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Run the scheduled tasks.
        /// </summary>
        /// <remarks>
        ///     This is the only situation in which an asynchronous method can be called
        ///     without returning a future. We know for sure that this method was executed
        ///     on a separate thread.
        /// </remarks>
        private async void Worker(object state)
        {
            _logger.LogTrace("Timed worker is running scheduled jobs.");

            using CancellationTokenSource cts = new(TimeSpan.FromSeconds(30));

            await signal.WaitAsync(cts.Token);

            try
            {
                using var scope = _servicesProvider.CreateScope();
                var backgroundTaskDispatcher = scope.ServiceProvider.GetRequiredService<BackgroundTaskScopedDispatcher>();

                foreach (var task in scheduleTasks.ToArray())
                {
                    await backgroundTaskDispatcher.EnqueueTaskAsync(task.TaskName, task.Value);
                }
            }
            finally
            {
                signal.Release();
            }
        }

        /// <summary>
        ///     Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        public void Dispose() => _timer?.Dispose();
    }
}