using FunderMaps.Core.Types.Distributions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Contract for a statistic analysis repository.
    /// </summary>
    public interface IStatisticsRepository
    {
        /// <summary>
        ///     Gets a <see cref="FoundationTypeDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="FoundationTypeDistribution"/></returns>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ConstructionYearDistribution"/></returns>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="FoundationRiskDistribution"/></returns>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets the collected data percentage by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Percentage</returns>
        Task<double> GetDataCollectedPercentageByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of buildings restored by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalBuildingRestoredCountByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of incidents by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalIncidentCountByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of reports by neighborhood.
        /// </summary>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalReportCountByIdAsync(Guid userId, string neighborhoodId, CancellationToken token = default);

        /// <summary>
        ///     Gets a <see cref="FoundationTypeDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="FoundationTypeDistribution"/></returns>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ConstructionYearDistribution"/></returns>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="FoundationRiskDistribution"/></returns>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets the collected data percentage by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Percentage</returns>
        Task<double> GetDataCollectedPercentageByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of buildings restored by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of incidents by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalIncidentCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);

        /// <summary>
        ///     Gets the amount of reports by neighborhood.
        /// </summary>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Count</returns>
        Task<uint> GetTotalReportCountByExternalIdAsync(Guid userId, string neighborhoodCode, CancellationToken token = default);
    }
}
