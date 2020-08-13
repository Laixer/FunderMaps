using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Distributions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// TODO Implement.
    /// <summary>
    ///     Repository for statistics.
    /// </summary>
    internal sealed class StatisticsRepository : IStatisticsRepository
    {
        public Task<ConstructionYearDistribution> GetConstructionYearDistributionAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<double> GetDataCollectedPercentageAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationRiskDistribution> GetFoundationRiskDistributionAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<FoundationTypeDistribution> GetFoundationTypeDistributionAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalBuildingRestoredCountAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalIncidentCountAsync(string municipalityId, CancellationToken token) => throw new NotImplementedException();
        public Task<uint> GetTotalReportCountAsync(string neighborhoodId, CancellationToken token) => throw new NotImplementedException();
    }
}
