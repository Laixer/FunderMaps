namespace FunderMaps.Worker;

public interface ISingleShotTask
{
    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RunAsync(CancellationToken cancellationToken);
}
