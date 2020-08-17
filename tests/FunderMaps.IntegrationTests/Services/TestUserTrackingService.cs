using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using System;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Services
{
    public class TestUserTrackingService : IUserTrackingService
    {
        public Task ProcessAnalysisRequest(Guid userId, AnalysisProductType productType) => Task.CompletedTask;
        public Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType) => Task.CompletedTask;
    }
}
