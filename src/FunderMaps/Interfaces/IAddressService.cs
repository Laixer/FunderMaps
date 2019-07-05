using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;

namespace FunderMaps.Interfaces
{
    public interface IAddressService
    {
        /// <summary>
        /// Find the address by id or combination of characteristics.
        /// </summary>
        /// <param name="address">Input entity.</param>
        /// <returns>Address.</returns>
        Task<Address> FindAddressAsync(Address address);
    }
}
