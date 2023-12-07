using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker;

/// <summary>
///     Construct new instance.
/// </summary>
public class TaskRunner(
    HealthCheckService healthCheckService,
    IServiceScopeFactory serviceScopeFactory,
    IEmailService emailService,
    IHostApplicationLifetime hostApplicationLifetime,
    IConfiguration configuration,
    ILogger<TaskRunner> logger) : IHostedService
{
    private readonly HealthCheckService _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly ILogger<TaskRunner> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
                if (string.IsNullOrEmpty(_configuration["Task"]))
                {
                    throw new InvalidOperationException("Batch task not specified");
                }

                switch (_configuration["Task"]?.ToLowerInvariant())
                {
                    case "loadbag":
                        {
                            _logger.LogInformation("Running loading BAG task");

                            using var scope = _serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<DownloadBagTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "refreshmodels":
                        {
                            _logger.LogInformation("Running refresh data models task");

                            using var scope = _serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<RefreshDataModelsTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "productexport":
                        {
                            _logger.LogInformation("Running product export task");

                            using var scope = _serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<ProductExportTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "mapbundle":
                        {
                            _logger.LogInformation("Running map bundle task");

                            using var scope = _serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<MapBundleTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    default:
                        throw new InvalidOperationException("Invalid batch task");
                }
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
}
