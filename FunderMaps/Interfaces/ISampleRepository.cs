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
        Task<IReadOnlyList<Sample>> ListAllAsync(Navigation navigation);
        Task<IReadOnlyList<Sample>> ListAllAsync(int org_id, Navigation navigation);
        Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, Navigation navigation);
        Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, int org_id, Navigation navigation);
        Task<int> CountAsync(int org_id);
    }
}
