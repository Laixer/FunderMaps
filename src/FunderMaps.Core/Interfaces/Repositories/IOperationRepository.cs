namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Various data operations.
/// </summary>
public interface IOperationRepository
{
    /// <summary>
    ///     Check if backend is online.
    /// </summary>
    Task<bool> IsAliveAsync();

    /// <summary>
    ///    Refresh data models.
    /// </summary>
    Task RefreshModelAsync();

    /// <summary>
    ///   Refresh statistics.
    /// </summary>
    Task RefreshStatisticsAsync();

    /// <summary>
    ///   Copy BAG data to building table.
    /// </summary>
    Task CopyPandToBuildingAsync();
}
