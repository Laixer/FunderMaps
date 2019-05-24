using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Interfaces;

namespace FunderMaps.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository) => _addressRepository = addressRepository;

        public Task<Address> FindAddressAsync(Address address)
        {
            return _addressRepository.GetOrAddAsync(address);
        }
    }
}
