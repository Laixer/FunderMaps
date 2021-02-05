using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using System.Linq;
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

        public TestStatisticsRepository(DataStore<StatisticsProduct> dataStore) => DataStore = dataStore;

        public Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<decimal> GetDataCollectedPercentageByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<decimal> GetDataCollectedPercentageByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalBuildingRestoredCountByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalBuildingRestoredCountByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalIncidentCountByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalIncidentCountByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalReportCountByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> GetTotalReportCountByExternalIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
