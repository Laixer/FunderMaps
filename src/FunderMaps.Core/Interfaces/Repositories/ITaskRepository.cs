namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the task repository.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    ///     Log the run time of a task.
    /// </summary>
    Task LogRunTimeAsync(string id, TimeSpan runtime);
}
