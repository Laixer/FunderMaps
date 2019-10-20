using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Address repository.
    /// </summary>
    public interface IAddressRepository : IAsyncRepository<Address, Guid>
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
