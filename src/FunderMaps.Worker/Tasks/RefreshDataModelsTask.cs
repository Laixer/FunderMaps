using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker.Tasks;

internal sealed class RefreshDataModelsTask(
    IOperationRepository operationRepository,
    ILogger<RefreshDataModelsTask> logger) : ITaskService
{
    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Refreshing model");

        await operationRepository.RefreshModelAsync();

        logger.LogInformation("Refreshing statistics");

        await operationRepository.RefreshStatisticsAsync();
    }
}
