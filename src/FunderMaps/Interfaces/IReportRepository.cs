using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the report repository.
    /// </summary>
    public interface IReportRepository : IAsyncRepository<Report, int>
    {
        /// <summary>
        /// Retrieve entity by id and document_id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="document">Document identifier.</param>
        /// <returns>Entity.</returns>
        Task<Report> GetByIdAsync(int id, string document);

        /// <summary>
        /// Retrieve all entities and filter on organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<Report>> ListAllAsync(int org_id, Navigation navigation);

        /// <summary>
        /// Retrieve number of entities and filter on organization id.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync(int org_id);
    }
}
