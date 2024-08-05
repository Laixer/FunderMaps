using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Log product hit.
/// </summary>
public interface ITelemetryRepository
{
    // TODO: Rename
    /// <summary>
    ///    Retrieve all product telemetrics.
    /// </summary>
    IAsyncEnumerable<Guid> ListLastMonthOrganizationaAsync();

    // TODO: Rename
    /// <summary>
    ///    Retrieve all product telemetrics.
    /// </summary>
    IAsyncEnumerable<ProductCall> ListLastMonthByOrganizationIdAsync(Guid id);
}
