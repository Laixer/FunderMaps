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
            //Address found_address = null;

            return _addressRepository.GetOrAddAsync(address);

            // Find address by id
            //if (address.Id != Guid.Empty)
            //{
            //    found_address = await _addressRepository.GetByIdAsync(address.Id);
            //}

            //// Find address by input address
            //if (found_address == null)
            //{
            //    found_address = await _addressRepository.GetByAddressAsync(address.StreetName,
            //        address.BuildingNumber,
            //        address.BuildingNumberSuffix);
            //}

            //// Add new address
            //if (found_address == null)
            //{
            //    found_address = await _addressRepository.AddAsync(address);
            //}

            //return found_address;
        }
    }
}
