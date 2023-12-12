using FunderMaps.Core.Interfaces;

namespace FunderMaps.Worker.Tasks;

internal sealed class RefreshDataModelsTask : ITaskService
{
    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
