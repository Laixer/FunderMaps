using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Worker.Tasks;

internal sealed class RefreshDataModelsTask(IOperationRepository operationRepository) : ITaskService
{
    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await operationRepository.RefreshModelAsync();
        await operationRepository.RefreshStatisticsAsync();
    }
}
