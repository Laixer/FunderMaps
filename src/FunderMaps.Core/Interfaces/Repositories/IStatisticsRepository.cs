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
        ///     Get statistics product by id.
        /// </summary>
        Task<StatisticsProduct> GetStatisticsProductByIdAsync(string id);

        /// <summary>
        ///     Get statistics product by external id.
        /// </summary>
        Task<StatisticsProduct> GetStatisticsProductByExternalIdAsync(string id);
    }
}
