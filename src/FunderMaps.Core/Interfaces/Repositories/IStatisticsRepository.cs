using FunderMaps.Core.Types.Products;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Contract for a statistic analysis repository.
    /// </summary>
    public interface IStatisticsRepository
    {
        /// <summary>
        ///     Fill all statistic fields if required.
        /// </summary>
        Task<StatisticsProduct> GetStatisticsProductByIdAsync(string id);

        /// <summary>
        ///     Fill all statistic fields if required.
        /// </summary>
        Task<StatisticsProduct> GetStatisticsProductByExternalIdAsync(string id);
    }
}
