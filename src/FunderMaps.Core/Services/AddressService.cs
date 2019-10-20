using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
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
        /// Find address by address object.
        /// </summary>
        /// <param name="address">Address object.</param>
        /// <returns><see cref="Address"/> or null.</returns>
        public Task<Address> GetOrCreateAddressAsync(Address address) => _addressRepository.GetOrAddAsync(address);
    }
}
