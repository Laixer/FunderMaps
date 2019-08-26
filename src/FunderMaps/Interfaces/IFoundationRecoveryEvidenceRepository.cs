using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Interface for the foundation recovery evidence repository
    /// </summary>
    public interface IFoundationRecoveryEvidenceRepository : IAsyncRepository<FoundationRecoveryEvidence, string>
    {
        /// <summary>
        /// Generate a list of evidence items based on the organization id
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        Task<IReadOnlyList<FoundationRecoveryEvidence>> ListAllAsync(int org_id, Navigation navigation);

        /// <summary>
        /// Return the amount of reports counts based on the organization id
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        Task<uint> CountAsync(int org_id);
    }
}
