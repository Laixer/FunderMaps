using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the sample repository.
    /// </summary>
    public interface ISampleRepository : IAsyncRepository<Sample, int>
    {
        /// <summary>
        /// Retrieve all entities and filter on organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<Sample>> ListAllAsync(int org_id, Navigation navigation);

        /// <summary>
        /// Retrieve all entities and filter on report.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, Navigation navigation);

        /// <summary>
        /// Retrieve all entities and filter on report and organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, int org_id, Navigation navigation);

        /// <summary>
        /// Retrieve number of entities and filter on organization id.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync(int org_id);
    }
}
