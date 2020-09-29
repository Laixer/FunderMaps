using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestTrackingRepository : ITrackingRepository
    {
        public Task ProcessAnalysisUsageAsync(Guid userId, AnalysisProductType product, uint itemCount = 1) => Task.CompletedTask;
        public Task ProcessStatisticsUsageAsync(Guid userId, StatisticsProductType product, uint itemCount = 1) => Task.CompletedTask;
    }
}
