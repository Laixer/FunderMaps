using FunderMaps.Core.Types.Products;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Contract for tracking user behaviour regarding requests.
    /// </summary>
    public interface IUserTrackingService
    {
        Task ProcessAnalysisRequest(Guid userId, AnalysisProductType productType);

        Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType);
    }
}
