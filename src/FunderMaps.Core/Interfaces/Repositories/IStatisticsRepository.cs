using FunderMaps.Core.Types.Distributions;
using System;
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
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets the collected data percentage by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <returns>Percentage</returns>
        Task<double> GetDataCollectedPercentageByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets the amount of buildings restored by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalBuildingRestoredCountByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets the amount of incidents by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalIncidentCountByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets the amount of reports by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodId">Internal neighborhood id.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalReportCountByIdAsync(Guid userId, string neighborhoodId);

        /// <summary>
        ///     Gets a <see cref="FoundationTypeDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/> by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets the collected data percentage by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <returns>Percentage</returns>
        Task<double> GetDataCollectedPercentageByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets the amount of buildings restored by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets the amount of incidents by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalIncidentCountByExternalIdAsync(Guid userId, string neighborhoodCode);

        /// <summary>
        ///     Gets the amount of reports by neighborhood.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <returns>Count</returns>
        Task<uint> GetTotalReportCountByExternalIdAsync(Guid userId, string neighborhoodCode);
    }
}
