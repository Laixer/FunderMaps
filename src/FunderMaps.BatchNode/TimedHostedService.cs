using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FunderMaps.Core.MapBundle;
using Microsoft.Extensions.Configuration;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Schedule tasks with interval.
    /// </summary>
    public class TimedHostedService : IHostedService, IAsyncDisposable
    {
        private readonly int intervalInMinutes;

        private readonly IServiceProvider _servicesProvider;
        private readonly ILogger<TimedHostedService> _logger;

        private Timer _timer;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TimedHostedService(IServiceProvider serviceProvider, ILogger<TimedHostedService> logger, IConfiguration configuration)
        {
            _servicesProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger = logger ?? throw new ArgumentNullException(nameof(logger));

            intervalInMinutes = configuration.GetValue<int>("Batch:Interval", 10);
        }

        /// <summary>
        ///     Triggered when the application host is ready to start the service.
        /// </summary>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new(Worker, null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(intervalInMinutes));

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
            _logger.LogTrace("Timed worker is running.");

            using var scope = _servicesProvider.CreateScope();
            var bundleService = scope.ServiceProvider.GetRequiredService<IBundleService>();

            await bundleService.BuildAsync();
        }

        /// <summary>
        ///     Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        public ValueTask DisposeAsync() => _timer.DisposeAsync();
    }
}