using System.Threading.Tasks;
using System.Collections.Generic;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;


namespace FunderMaps.Interfaces
{
    public interface IFoundationRecoveryRepository : IAsyncRepository<FoundationRecovery>
    {
        /// <summary>
        /// Admin function. List EVERY THING based on the navigation parameter
        /// </summary>
        /// <param name="navigation">The navigation parameters</param>
        /// <returns></returns>
        Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Navigation navigation);

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
        Task<int> CountAsync(int org_id);

        /// <summary>
        /// Updates the delete date of a record
        /// </summary>
        /// <param name="report">entitiy to delete</param>
        /// <returns></returns>
        Task<int> DeleteAsync(FoundationRecovery report);
    }
}
