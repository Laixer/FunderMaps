using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface IPrincipalRepository : IAsyncRepository<Principal, int>
    {
        /// <summary>
        /// Get existing principal entity from data store or insert given
        /// as new entity.
        /// </summary>
        /// <param name="principal">Input principal.</param>
        /// <returns>See <see cref="Principal"/>.</returns>
        Task<Principal> GetOrAddAsync(Principal principal);
    }
}
