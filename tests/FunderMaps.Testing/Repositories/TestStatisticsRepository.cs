using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    /// <summary>
    ///     Mockup <see cref="StatisticsProduct"/> repository.
    /// </summary>
    public class TestStatisticsRepository : IStatisticsRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public DataStore<StatisticsProduct> DataStore { get; set; }

        public TestStatisticsRepository(DataStore<StatisticsProduct> dataStore)
        {
            DataStore = dataStore;
        }

        private StatisticsProduct FromNeighborhoodId(string neighboorhoodId)
            => DataStore.ItemList.Where(x => x.NeighborhoodId == neighboorhoodId).FirstOrDefault();

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.ConstructionYearDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.ConstructionYearDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<double> GetDataCollectedPercentageByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.DataCollectedPercentage ?? throw new EntityNotFoundException();
        }

        public async Task<double> GetDataCollectedPercentageByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.DataCollectedPercentage ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.FoundationRiskDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationRiskDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationTypeDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.FoundationTypeDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalBuildingRestoredCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalBuildingRestoredCountByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalBuildingRestoredCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalIncidentCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalIncidentCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalIncidentCountByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalIncidentCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalReportCountByExternalIdAsync(string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalReportCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalReportCountByIdAsync(string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalReportCount ?? throw new EntityNotFoundException();
        }
    }
}
