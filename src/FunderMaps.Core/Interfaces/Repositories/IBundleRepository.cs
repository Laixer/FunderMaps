using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository for bundles.
    /// </summary>
    public interface IBundleRepository : IAsyncRepository<Bundle, Guid>
    {
        /// <summary>
        ///     Get all bundles for a given organization.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="navigation">Navigation.</param>
        /// <returns>Collection of <see cref="Bundle"/> entities.</returns>
        IAsyncEnumerable<Bundle> GetAllByOrganizationAsync(Guid organizationId, INavigation navigation);

        /// <summary>
        ///     Forces a bundles version id to be refreshed. This should be used
        ///     to force bundle recalculations without messing up versions.
        /// </summary>
        /// <param name="bundleId">The bundle id.</param>
        /// <returns>The new version id.</returns>
        Task<uint> ForceUpdateBundleVersionAsync(Guid bundleId);
    }
}
