using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface ISampleRepository : IAsyncRepository<Sample>
    {
        Task<Sample> GetByIdWithItemsAsync(int id);
    }
}
