using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    /// Address service.
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="addressRepository"></param>
        public AddressService(IAddressRepository addressRepository) => _addressRepository = addressRepository;

        /// <summary>
        /// Find all addresses matching on streetname.
        /// </summary>
        /// <param name="streetName">Street name suggestion.</param>
        /// <returns>Id of first record.</returns>
        public async Task<IEnumerable<Address2>> GetAddressByStreetNameAsync(string streetName)
        {
            if (string.IsNullOrEmpty(streetName))
            {
                return null;
            }

            var addressMatch = await _addressRepository.GetByStreetNameAsync(streetName, 50);
            if (addressMatch is null)
            {
                return null;
            }

            // FUTURE: Cache these results temporary.

            return addressMatch.ToList();
        }

        /// <summary>
        /// Find address by address object.
        /// </summary>
        /// <param name="address">Address object.</param>
        /// <returns><see cref="Address"/> or null.</returns>
        public Task<Address> GetOrCreateAddressAsync(Address address) => _addressRepository.GetOrAddAsync(address);
    }
}
