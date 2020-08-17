using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    /// TODO Implement.
    /// <summary>
    ///     Repository for statistics.
    /// </summary>
    internal sealed class StatisticsRepository : IStatisticsRepository
    {
        public Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<double> GetDataCollectedPercentageByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<double> GetDataCollectedPercentageByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalBuildingRestoredCountByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalIncidentCountByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalIncidentCountByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalReportCountByExternalIdAsync(string neighborhoodCode, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalReportCountByIdAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
    }
}
#pragma warning disable CA1812 // Internal class is never instantiated
