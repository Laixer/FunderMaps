using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Log product hit.
    /// </summary>
    public interface ITelemetryRepository
    {
        /// <summary>
        ///     Retrieve all product telemetrics.
        /// </summary>
        IAsyncEnumerable<ProductTelemetry> ListAllUsageAsync();
    }
}
