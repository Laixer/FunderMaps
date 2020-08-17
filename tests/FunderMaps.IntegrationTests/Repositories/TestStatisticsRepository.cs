using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace FunderMaps.IntegrationTests.Repositories
{
    /// TODO Fill empty objects.
    /// TODO Do we want this returning format?
    /// <summary>
    ///     Mockup <see cref="StatisticsProduct"/> repository.
    /// </summary>
    public class TestStatisticsRepository : TestObjectRepositoryBase<StatisticsProduct>, IStatisticsRepository
    {
        public TestStatisticsRepository(ObjectDataStore dataStore) : base(dataStore) { }

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.ConstructionYearDistribution ?? throw new EntityNotFoundException();

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.ConstructionYearDistribution ?? throw new EntityNotFoundException();

        public async Task<double> GetDataCollectedPercentageByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.DataCollectedPercentage ?? throw new EntityNotFoundException();

        public async Task<double> GetDataCollectedPercentageByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.DataCollectedPercentage ?? throw new EntityNotFoundException();

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.FoundationRiskDistribution ?? throw new EntityNotFoundException();

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationRiskDistribution ?? throw new EntityNotFoundException();

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationTypeDistribution ?? throw new EntityNotFoundException();

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.FoundationTypeDistribution ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalBuildingRestored ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalBuildingRestoredCountByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.TotalBuildingRestored ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalIncidentCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalIncidents ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalIncidentCountByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.TotalIncidents ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalReportCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalReportCount ?? throw new EntityNotFoundException();

        public async Task<uint> GetTotalReportCountByIdAsync(string neighboorhoodId, CancellationToken token)
            => DataStore.GetObjectsFromType<StatisticsProduct>().Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault()?.TotalReportCount ?? throw new EntityNotFoundException();
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
