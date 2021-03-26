using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the test repository.
    /// </summary>
    public interface ITestRepository
    {
        /// <summary>
        ///     Check if backend is online.
        /// </summary>
        Task<bool> IsAliveAsync();
    }
}
