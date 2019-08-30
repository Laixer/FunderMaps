using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the foundation recovery repository.
    /// </summary>
    public interface IFoundationRecoveryRepository : IAsyncRepository<FoundationRecovery, int>
    {
        /// <summary>
        /// Return a list of items filterd by the organization id and the navigation values
        /// </summary>
        /// <param name="org_id">The id of the organization</param>
        /// <param name="navigation">The Navigation values</param>
        /// <returns></returns>
        Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(int org_id, Navigation navigation);

        /// <summary>
        /// Return amount counted based on organization Id
        /// </summary>
        /// <param name="org_id">Id of the organization</param>
        /// <returns></returns>
        Task<uint> CountAsync(int org_id);
    }
}
