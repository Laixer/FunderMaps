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
        ///     Marks a bundle as processing.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        Task MarkAsProcessingAsync(Guid bundleId);

        /// <summary>
        ///     Marks a bundle as up to date.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        Task MarkAsUpToDateAsync(Guid bundleId);
    }
}
