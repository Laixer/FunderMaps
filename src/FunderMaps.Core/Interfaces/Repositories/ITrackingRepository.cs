using FunderMaps.Core.Types.Products;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Contract for tracking user behaviour with regards to products.
    /// </summary>
    public interface ITrackingRepository
    {
        /// <summary>
        ///     Process analysis product usage.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="itemCount">Result set item count.</param>
        Task ProcessAnalysisUsageAsync(Guid userId, AnalysisProductType product, uint itemCount = 1);

        /// <summary>
        ///     Process statistics product usage.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="itemCount">Result set item count.</param>
        Task ProcessStatisticsUsageAsync(Guid userId, StatisticsProductType product, uint itemCount = 1);
    }
}
