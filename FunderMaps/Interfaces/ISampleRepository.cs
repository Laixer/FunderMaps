using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface ISampleRepository : IAsyncRepository<Sample>
    {
        Task<IReadOnlyList<Sample>> ListAllPublicAsync(int org_id, int offset, int limit);
        Task<Sample> GetByIdWithItemsAsync(int id);
    }
}
