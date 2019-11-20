using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Address repository.
    /// </summary>
    public interface IAddressRepository : IAsyncRepository<Address, Guid>
    {
        /// <summary>
        /// Get existing address entity from data store or insert given as new entity.
        /// </summary>
        /// <param name="address">Input address.</param>
        /// <returns>See <see cref="Address"/>.</returns>
        Task<Address> GetOrAddAsync(Address address);

        /// <summary>
        /// Get all adresses matching the search criteria.
        /// </summary>
        /// <param name="streetName">Partial street name.</param>
        /// <param name="limit">Max number or results.</param>
        /// <returns>List of <see cref="Address2"/>.</returns>
        Task<IReadOnlyList<Address2>> GetByStreetNameAsync(string streetName, uint limit = 100);
    }
}
