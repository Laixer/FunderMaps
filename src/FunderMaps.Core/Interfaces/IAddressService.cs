using FunderMaps.Core.Entities;
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
    }
}
