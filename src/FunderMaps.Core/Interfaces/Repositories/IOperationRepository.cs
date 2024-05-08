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
    ///     Refresh data models.
    /// </summary>
    Task RefreshModelAsync();

    /// <summary>
    ///     Refresh statistics.
    /// </summary>
    Task RefreshStatisticsAsync();

    /// <summary>
    ///     Cleanup BAG data.
    /// </summary>
    Task CleanupBAGAsync();

    /// <summary>
    ///     Load building data from BAG.
    /// </summary>
    Task LoadBuildingAsync();

    /// <summary>
    ///     Load address data from BAG.
    /// </summary>
    Task LoadAddressAsync();

    /// <summary>
    ///     Load residence data from BAG.
    /// </summary>
    Task LoadResidenceAsync();
}
