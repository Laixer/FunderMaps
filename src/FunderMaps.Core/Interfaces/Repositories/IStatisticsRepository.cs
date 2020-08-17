using FunderMaps.Core.Types.Distributions;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string neighborhoodId, CancellationToken token);

        Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string neighborhoodId, CancellationToken token);

        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string neighborhoodId, CancellationToken token);

        Task<double> GetDataCollectedPercentageByIdAsync(string neighborhoodId, CancellationToken token);

        Task<uint> GetTotalBuildingRestoredCountByIdAsync(string neighborhoodId, CancellationToken token);

        Task<uint> GetTotalIncidentCountByIdAsync(string neighborhoodId, CancellationToken token);

        Task<uint> GetTotalReportCountByIdAsync(string neighborhoodId, CancellationToken token);

        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<double> GetDataCollectedPercentageByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<uint> GetTotalIncidentCountByExternalIdAsync(string neighborhoodCode, CancellationToken token);

        Task<uint> GetTotalReportCountByExternalIdAsync(string neighborhoodCode, CancellationToken token);
    }
}
