using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository for bundles.
    /// </summary>
    public interface IBundleRepository : IAsyncRepository<Bundle, Guid>
    {
        /// <summary>
        ///     Get all recent changed bundles in our database.
        /// </summary>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns>Collection of bundles.</returns>
        IAsyncEnumerable<Bundle> ListAllRecentAsync(INavigation navigation);
    }
}
