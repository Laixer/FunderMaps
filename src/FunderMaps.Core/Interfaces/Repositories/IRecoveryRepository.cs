using FunderMaps.Core.Entities;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the recovery repository.
    /// </summary>
    public interface IRecoveryRepository : IAsyncRepository<Recovery, int>
    {
        /// <summary>
        ///     Set <see cref="Recovery"/> audit status.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="entity">Entity object.</param>
        Task SetAuditStatusAsync(int id, Recovery entity);
    }
}
