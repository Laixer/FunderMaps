using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Services
{
    public class TestUserTrackingService : IUserTrackingService
    {
        public Task ProcessMultipleAnalysisRequest(Guid userId, AnalysisProductType productType, uint itemCount, CancellationToken token) => Task.CompletedTask;
        public Task ProcessSingleAnalysisRequest(Guid userId, AnalysisProductType productType, CancellationToken token) => Task.CompletedTask;
        public Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType, CancellationToken token) => Task.CompletedTask;
    }
}
