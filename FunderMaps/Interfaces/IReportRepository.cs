using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;

namespace FunderMaps.Interfaces
{
    public interface IReportRepository : IAsyncRepository<Report>
    {
        Task<Report> GetByIdAsync(int id, string document);
        Task<IReadOnlyList<Report>> ListAllAsync(Navigation navigation);
        Task<IReadOnlyList<Report>> ListAllAsync(int org_id, Navigation navigation);
        Task<int> CountAsync(int org_id);
    }
}
