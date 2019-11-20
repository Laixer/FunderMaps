using FunderMaps.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Address service.
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Find the address by id or combination of characteristics.
        /// </summary>
        /// <param name="address">Input entity.</param>
        /// <returns>Address.</returns>
        Task<Address> GetOrCreateAddressAsync(Address address);

        /// <summary>
        /// Find all addresses matching on streetname.
        /// </summary>
        /// <param name="streetName">Street name suggestion.</param>
        /// <returns>Id of first record.</returns>
        Task<IEnumerable<Address2>> GetAddressByStreetNameAsync(string streetName);

        /// <summary>
        /// Add address if it does not exist yet.
        /// </summary>
        /// <param name="address">See <see cref="Address2"/>.</param>
        Task AddAddressIfNotExist(Address2 address);
    }
}
