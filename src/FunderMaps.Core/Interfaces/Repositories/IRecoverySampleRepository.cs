using FunderMaps.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the recovery sample repository.
    /// </summary>
    public interface IRecoverySampleRepository : IAsyncRepository<RecoverySample, int>
    {
        /// <summary>
        ///     Retrieve number of <see cref="RecoverySample"/> for a given <see cref="Recovery"/>.
        /// </summary>
        /// <returns>Number of <see cref="RecoverySample"/>.</returns>
        Task<long> CountAsync(int recovery);

        /// <summary>
        ///     Retrieve all <see cref="RecoverySample"/> for a <see cref="Recovery"/>.
        /// </summary>
        /// <returns>List of <see cref="RecoverySample"/>.</returns>
        IAsyncEnumerable<RecoverySample> ListAllAsync(int recovery, Navigation navigation);
    }
}
