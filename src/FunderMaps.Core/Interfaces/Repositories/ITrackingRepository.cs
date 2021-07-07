using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Log product hit.
    /// </summary>
    public interface ITelemetryRepository
    {
        /// <summary>
        ///     Log a product hit.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <param name="hitCount">Number of hits to log.</param>
        Task ProductHitAsync(string productName, int hitCount = 1);

        /// <summary>
        ///     Retrieve all product telemetrics.
        /// </summary>
        IAsyncEnumerable<ProductTelemetry> ListAllUsageAsync();
    }
}
