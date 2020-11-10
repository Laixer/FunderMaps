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
        ///     Marks a bundle as processing.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        Task MarkAsProcessingAsync(Guid bundleId);

        /// <summary>
        ///     Marks a bundle as up to date and pump the version.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        Task MarkAsUpToDateBumpVersionAsync(Guid bundleId);
    }
}
