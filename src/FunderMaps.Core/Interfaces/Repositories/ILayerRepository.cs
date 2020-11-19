using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository for layers.
    /// </summary>
    public interface ILayerRepository : IAsyncRepository<Layer, Guid>
    {
        /// <summary>
        ///     Retrieve all layers that are linked with a <paramref name="bundleId"/>.
        /// </summary>
        /// <param name="bundleId">The linked bundle id.</param>
        IAsyncEnumerable<Layer> ListAllFromBundleIdAsync(Guid bundleId);
    }
}
