using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository for layers.
    /// </summary>
    public interface ILayerRepository
    {
        /// <summary>
        ///     Retrieve layer by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Layer"/>.</returns>
        ValueTask<Layer> GetByIdAsync(Guid id);

        /// <summary>
        ///     Retrieve all layers.
        /// </summary>
        /// <returns>List of <see cref="Layer"/>.</returns>
        IAsyncEnumerable<Layer> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     Retrieve all layers that are linked with a <paramref name="bundleId"/>.
        /// </summary>
        /// <param name="bundleId">The linked bundle id.</param>
        IAsyncEnumerable<Layer> ListAllFromBundleIdAsync(Guid bundleId);
    }
}
