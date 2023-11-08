using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunderMaps.ProductExporter;

public abstract class SingleShotService : IHostedService
{
    protected readonly HealthCheckService _healthCheckService;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly IEmailService _emailService;
    protected readonly IHostApplicationLifetime _hostApplicationLifetime;
    protected readonly ILogger _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public SingleShotService(
        HealthCheckService healthCheckService,
        IServiceScopeFactory serviceScopeFactory,
        IEmailService emailService,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger logger)
    {
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            if (healthReport.Status != HealthStatus.Healthy)
            {
                _logger.LogError("Health check failed, stopping application");

                throw new InvalidOperationException("Health check failed, stopping application");
            }
            else
            {
                using var scope = _serviceScopeFactory.CreateScope();

                await RunAsync(scope, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while running service");

            await _emailService.SendAsync(new EmailMessage
            {
                Subject = "Error while running service",
                Content = exception.Message,
            });
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="scope">Service scope.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected abstract Task RunAsync(IServiceScope scope, CancellationToken cancellationToken);
}
