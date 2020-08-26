using FunderMaps.Core.Types.Products;
using System;
using System.Threading;
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
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <param name="itemCount">Result set item count.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task ProcessAnalysisUsageAsync(Guid userId, AnalysisProductType product, uint itemCount = 1, CancellationToken token = default);

        /// <summary>
        ///     Process statistics product usage.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="product"><see cref="StatisticsProductType"/></param>
        /// <param name="itemCount">Result set item count.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task ProcessStatisticsUsageAsync(Guid userId, StatisticsProductType product, uint itemCount = 1, CancellationToken token = default);
    }
}
