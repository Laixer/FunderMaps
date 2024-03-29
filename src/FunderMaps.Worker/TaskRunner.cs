using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Worker.Tasks;
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
    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var healthReport = await healthCheckService.CheckHealthAsync(cancellationToken);
            if (healthReport.Status != HealthStatus.Healthy)
            {
                logger.LogError("Health check failed, stopping application");

                throw new InvalidOperationException("Health check failed, stopping application");
            }
            else
            {
                if (string.IsNullOrEmpty(configuration["Task"]))
                {
                    throw new InvalidOperationException("Batch task not specified");
                }

                switch (configuration["Task"]?.ToLowerInvariant())
                {
                    case "loadbag":
                        {
                            logger.LogInformation("Running loading BAG task");

                            using var scope = serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<LoadBagTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "refreshmodels":
                        {
                            logger.LogInformation("Running refresh data models task");

                            using var scope = serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<RefreshDataModelsTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "modelexport":
                        {
                            logger.LogInformation("Running model export task");

                            using var scope = serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<ModelExportTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "productexport":
                        {
                            logger.LogInformation("Running product export task");

                            using var scope = serviceScopeFactory.CreateScope();
                            await ActivatorUtilities.CreateInstance<ProductExportTask>(scope.ServiceProvider).RunAsync(cancellationToken);
                            break;
                        }

                    case "mapbundle":
                        {
                            logger.LogInformation("Running map bundle task");

                            using var scope = serviceScopeFactory.CreateScope();
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
            logger.LogError(exception, "Error while running service");

            await emailService.SendAsync(new EmailMessage
            {
                Subject = "Error while running service",
                Content = exception.Message,
            }, cancellationToken);
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
