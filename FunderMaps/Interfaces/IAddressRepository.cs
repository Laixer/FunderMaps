using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface IAddressRepository : IAsyncRepository<Address>
    {
        Task<Address> GetByIdAsync(Guid id);
        Task<Address> GetByAddressAsync(string street, short building_number, string building_number_suffix);
    }
}
