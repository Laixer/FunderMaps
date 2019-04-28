using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface IAddressRepository : IAsyncRepository<Address>
    {
        /// <summary>
        /// Get existing address entity from data store or insert given
        /// as new entity.
        /// </summary>
        /// <param name="address">Input address.</param>
        /// <returns>See <see cref="Address"/>.</returns>
        Task<Address> GetOrAddAsync(Address address);
    }
}
