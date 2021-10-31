using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using FunderMaps.Core.Model;

namespace FunderMaps.BatchNode;

/// <summary>
///     Schedule tasks with interval.
/// </summary>
public class TimedModelService : IHostedService, IAsyncDisposable
{
    private readonly ModelOptions _options;

    private readonly IServiceProvider _servicesProvider;
    private readonly ILogger<TimedModelService> _logger;

    private Timer _timer;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public TimedModelService(IServiceProvider serviceProvider,
        ILogger<TimedModelService> logger,
        IConfiguration configuration,
        IOptions<ModelOptions> options)
    {
        _servicesProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new(Worker, null, TimeSpan.Zero, TimeSpan.FromHours(_options.Interval));

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

        using IServiceScope scope = _servicesProvider.CreateScope();
        var modelService = scope.ServiceProvider.GetRequiredService<IModelService>();

        await modelService.UpdateAllModelsAsync();
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
