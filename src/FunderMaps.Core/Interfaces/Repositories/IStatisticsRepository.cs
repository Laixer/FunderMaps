using FunderMaps.Core.Types.Distributions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Contract for a statistic analysis repository.
    /// </summary>
    public interface IStatisticsRepository
    {
        /// <summary>
        ///     Get foundation type distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string id);

        /// <summary>
        ///     Get foundation type distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string id);

        /// <summary>
        ///     Get construction year distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string id);

        /// <summary>
        ///     Get construction year distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string id);

        /// <summary>
        ///     Get data collection percentage by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<decimal> GetDataCollectedPercentageByIdAsync(string id);

        /// <summary>
        ///     Get data collection percentage by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<decimal> GetDataCollectedPercentageByExternalIdAsync(string id);

        /// <summary>
        ///     Get foundation risk distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string id);

        /// <summary>
        ///     Get foundation risk distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string id);

        /// <summary>
        ///     Get total building restored count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<long> GetTotalBuildingRestoredCountByIdAsync(string id);

        /// <summary>
        ///     Get total building restored count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<long> GetTotalBuildingRestoredCountByExternalIdAsync(string id);

        /// <summary>
        ///     Get total incident count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<IEnumerable<IncidentYearPair>> GetTotalIncidentCountByIdAsync(string id);

        /// <summary>
        ///     Get total incident count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<IEnumerable<IncidentYearPair>> GetTotalIncidentCountByExternalIdAsync(string id);

        /// <summary>
        ///     Get total incident count by id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        Task<IEnumerable<IncidentYearPair>> GetMunicipalityIncidentCountByIdAsync(string id);

        /// <summary>
        ///     Get total incident count by external id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        Task<IEnumerable<IncidentYearPair>> GetMunicipalityIncidentCountByExternalIdAsync(string id);

        /// <summary>
        ///     Get total report count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<List<InquiryYearPair>> GetTotalReportCountByIdAsync(string id);

        /// <summary>
        ///     Get total report count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        Task<List<InquiryYearPair>> GetTotalReportCountByExternalIdAsync(string id);

        /// <summary>
        ///     Get total report count by id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        Task<IEnumerable<InquiryYearPair>> GetMunicipalityReportCountByIdAsync(string id);

        /// <summary>
        ///     Get total report count by external id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        Task<IEnumerable<InquiryYearPair>> GetMunicipalityReportCountByExternalIdAsync(string id);
    }
}
