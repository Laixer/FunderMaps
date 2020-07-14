using FunderMaps.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Address repository.
    /// </summary>
    public interface IAddressRepository : IAsyncRepository<Address, string>
    {
        /// <summary>
        /// Get all adresses matching postal code.
        /// </summary>
        /// <param name="postalCode">Exact postcode match.</param>
        /// <param name="navigation">Return set by navigation.</param>
        /// <returns>List of <see cref="Address"/>.</returns>
        IAsyncEnumerable<Address> GetByPostalCodeAsync(string postalCode, INavigation navigation);

        /// <summary>
        /// Get all adresses matching postal code.
        /// </summary>
        /// <param name="postalCode">Exact postcode match.</param>
        /// <param name="buildingNumber">Exact building number match.</param>
        /// <param name="navigation">Return set by navigation.</param>
        /// <returns>List of <see cref="Address"/>.</returns>
        IAsyncEnumerable<Address> GetByPostalCodeAsync(string postalCode, string buildingNumber, INavigation navigation);

        /// <summary>
        /// Get address by external id.
        /// </summary>
        /// <param name="id">External identifier.</param>
        /// <param name="source">External source.</param>
        /// <returns>A single address.</returns>
        Task<Address> GetByExternalIdAsync(string id, string source);
    }
}
