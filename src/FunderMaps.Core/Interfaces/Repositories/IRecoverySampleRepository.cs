using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the recovery sample repository.
    /// </summary>
    public interface IRecoverySampleRepository : IAsyncRepository<RecoverySample, int>
    {
    }
}
