using FunderMaps.Core.Types.Products;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Contract for tracking user behaviour regarding requests.
    /// </summary>
    public interface IUserTrackingService
    {
        Task ProcessSingleAnalysisRequest(Guid userId, AnalysisProductType productType, CancellationToken token);

        Task ProcessMultipleAnalysisRequest(Guid userId, AnalysisProductType productType, uint itemCount, CancellationToken token);

        Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType, CancellationToken token);
    }
}
