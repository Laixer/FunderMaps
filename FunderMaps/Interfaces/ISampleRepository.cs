using System;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using FunderMaps.Models.Fis;

namespace FunderMaps.Interfaces
{
    public interface ISampleRepository : IAsyncRepository<Sample>
    {
        Task<Sample> GetByIdWithItemsAsync(int id);
    }
}
