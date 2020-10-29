namespace FunderMaps.Console.Types
{
    /// <summary>
    ///     Base class for a synchronous background task.
    /// </summary>
    /// <remarks>
    ///     This task can also contain asynchronous operations,
    ///     but must have at least one synchronous operation.
    /// </remarks>
    public abstract class BackgroundTaskSynchronous : BackgroundTask
    {
    }
}
