using FunderMaps.Core.Types.Distributions;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionAsync(string neighborhoodId, CancellationToken token);

        Task<ConstructionYearDistribution> GetConstructionYearDistributionAsync(string neighborhoodId, CancellationToken token);

        Task<FoundationRiskDistribution> GetFoundationRiskDistributionAsync(string neighborhoodId, CancellationToken token);

        Task<double> GetDataCollectedPercentageAsync(string neighborhoodId, CancellationToken token);

        Task<uint> GetTotalBuildingRestoredCountAsync(string neighborhoodId, CancellationToken token);

        Task<uint> GetTotalIncidentCountAsync(string municipalityId, CancellationToken token);

        Task<uint> GetTotalReportCountAsync(string neighborhoodId, CancellationToken token);
    }
}
