using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;

namespace FunderMaps.Interfaces
{
    public interface ISampleRepository : IAsyncRepository<Sample2>
    {
        Task<IReadOnlyList<Sample2>> ListAllAsync(Navigation navigation);
        Task<IReadOnlyList<Sample2>> ListAllAsync(int org_id, Navigation navigation);
        Task<IReadOnlyList<Sample2>> ListAllReportAsync(int report, Navigation navigation);
        Task<IReadOnlyList<Sample2>> ListAllReportAsync(int report, int org_id, Navigation navigation);
        Task<int> CountAsync(int org_id, IDbConnection connection = null);
    }
}
