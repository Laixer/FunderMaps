using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using System;
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

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.ConstructionYearDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.ConstructionYearDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<double> GetDataCollectedPercentageByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.DataCollectedPercentage ?? throw new EntityNotFoundException();
        }

        public async Task<double> GetDataCollectedPercentageByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.DataCollectedPercentage ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.FoundationRiskDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationRiskDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.FoundationTypeDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.FoundationTypeDistribution ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalBuildingRestoredCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalBuildingRestoredCountByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalBuildingRestoredCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalIncidentCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalIncidentCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalIncidentCountByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalIncidentCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalReportCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.NeighborhoodCode == neighborhoodCode).FirstOrDefault()?.TotalReportCount ?? throw new EntityNotFoundException();
        }

        public async Task<uint> GetTotalReportCountByIdAsync(Guid userId, string neighboorhoodId, CancellationToken token)
        {
            await Task.CompletedTask;
            return FromNeighborhoodId(neighboorhoodId)?.TotalReportCount ?? throw new EntityNotFoundException();
        }
    }
}
