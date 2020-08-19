using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// TODO Implement
    /// <summary>
    ///     Service for tracking user behaviour with regards to products.
    /// </summary>
    public sealed class UserTrackingService : IUserTrackingService
    {
        public Task ProcessAnalysisRequest(Guid userId, AnalysisProductType productType) => Task.CompletedTask;
        public Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType) => Task.CompletedTask;
    }
}
