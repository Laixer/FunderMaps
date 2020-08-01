using FunderMaps.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Address repository.
    /// </summary>
    public interface IAddressRepository : IAsyncRepository<Address, string>
    {
        /// <summary>
        ///     Get all adresses matching query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="navigation">Return set by navigation.</param>
        /// <returns>List of <see cref="Address"/>.</returns>
        IAsyncEnumerable<Address> GetBySearchQueryAsync(string query, INavigation navigation);

        /// <summary>
        ///     Get address by external id.
        /// </summary>
        /// <param name="id">External identifier.</param>
        /// <param name="source">External source.</param>
        /// <returns>A single address.</returns>
        ValueTask<Address> GetByExternalIdAsync(string id, string source);
    }
}
