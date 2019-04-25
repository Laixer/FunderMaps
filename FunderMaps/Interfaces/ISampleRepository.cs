using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;

namespace FunderMaps.Interfaces
{
    public interface ISampleRepository : IAsyncRepository<Sample>
    {
        Task<IReadOnlyList<Sample>> ListAllAsync(int org_id, Navigation navigation);
    }
}
