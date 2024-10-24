using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

/// <summary>
///     Model service.
/// </summary>
/// <param name="geocoderTranslation">Geocoder translation service.</param>
/// <param name="analysisRepository">Analysis repository.</param>
/// <param name="statisticsRepository">Statistics repository.</param>
/// <param name="organizationRepository">Organization repository.</param>
/// <param name="logger">Logger.</param>
public class ModelService(
    GeocoderTranslation geocoderTranslation,
    IAnalysisRepository analysisRepository,
    IStatisticsRepository statisticsRepository,
    IOrganizationRepository organizationRepository,
    ILogger<ModelService> logger)
{
    /// <summary>
    ///    Get analysis product.
    /// </summary>
    /// <remarks>
    ///     The analysis product is retrieved by any identifier. The identifier is translated to a building id.
    /// </remarks>
    /// <param name="id">External identifier.</param>
    /// <param name="tenantId">Tenant identifier.</param>
    /// <param name="track_request">Tracker request.</param>
    public async Task<AnalysisProduct> GetAnalysisAsync(string id, Guid tenantId, bool track_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var product = await analysisRepository.GetAsync(building.Id);

            if (track_request)
            {
                var registered = await analysisRepository.RegisterProductMatch(building.Id, id, "analysis3", tenantId);
                if (registered)
                {
                    logger.LogInformation("{Name} registered 'analysis3' match for identifier: {id}", organization.Name, id);
                }
                else
                {
                    logger.LogInformation("{Name} retrieved 'analysis3' match for identifier: {id}", organization.Name, id);
                }
            }

            return product;
        }
        catch (EntityNotFoundException)
        {
            if (track_request)
            {
                await analysisRepository.RegisterProductMismatch(id, tenantId);

                logger.LogInformation("{Name} requested product 'analysis3' mismatch for identifier: {id}", organization.Name, id);
            }

            throw;
        }
    }

    /// <summary>
    ///    Get statistics product.
    /// </summary>
    /// <remarks>
    ///    The statistics product is retrieved by any identifier. The identifier is translated to a neighborhood id.
    /// </remarks>
    /// <param name="id">External identifier.</param>
    /// <param name="tenantId">Tenant identifier.</param>
    /// <param name="track_request">Tracker request.</param>
    public async Task<StatisticsProduct> GetStatisticsAsync(string id, Guid tenantId, bool track_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var neighborhood = await geocoderTranslation.GetNeighborhoodIdAsync(id);

            return new StatisticsProduct
            {
                FoundationTypeDistribution = await statisticsRepository.GetFoundationTypeDistributionByIdAsync(neighborhood.Id),
                ConstructionYearDistribution = await statisticsRepository.GetConstructionYearDistributionByIdAsync(neighborhood.Id),
                DataCollectedPercentage = await statisticsRepository.GetDataCollectedPercentageByIdAsync(neighborhood.Id),
                FoundationRiskDistribution = await statisticsRepository.GetFoundationRiskDistributionByIdAsync(neighborhood.Id),
                TotalBuildingRestoredCount = await statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(neighborhood.Id),
                TotalIncidentCount = await statisticsRepository.GetTotalIncidentCountByIdAsync(neighborhood.Id),
                MunicipalityIncidentCount = await statisticsRepository.GetMunicipalityIncidentCountByIdAsync(neighborhood.Id),
                TotalReportCount = await statisticsRepository.GetTotalReportCountByIdAsync(neighborhood.Id),
                MunicipalityReportCount = await statisticsRepository.GetMunicipalityReportCountByIdAsync(neighborhood.Id),
            };
        }
        catch (EntityNotFoundException)
        {
            if (track_request)
            {
                await analysisRepository.RegisterProductMismatch(id, tenantId);

                logger.LogInformation("{Name} requested product 'statistics' mismatch for identifier: {id}", organization.Name, id);
            }

            throw;
        }
    }

    // TODO: This is a temporary solution. The statistics product should be retrieved by building id.
    public async Task<StatisticsProduct> GetStatistics2Async(string id, Guid tenantId, bool track_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var neighborhood = building.NeighborhoodId ?? throw new EntityNotFoundException();

            return new StatisticsProduct
            {
                FoundationTypeDistribution = await statisticsRepository.GetFoundationTypeDistributionByIdAsync(neighborhood),
                ConstructionYearDistribution = await statisticsRepository.GetConstructionYearDistributionByIdAsync(neighborhood),
                DataCollectedPercentage = await statisticsRepository.GetDataCollectedPercentageByIdAsync(neighborhood),
                FoundationRiskDistribution = await statisticsRepository.GetFoundationRiskDistributionByIdAsync(neighborhood),
                TotalBuildingRestoredCount = await statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(neighborhood),
                TotalIncidentCount = await statisticsRepository.GetTotalIncidentCountByIdAsync(neighborhood),
                MunicipalityIncidentCount = await statisticsRepository.GetMunicipalityIncidentCountByIdAsync(neighborhood),
                TotalReportCount = await statisticsRepository.GetTotalReportCountByIdAsync(neighborhood),
                MunicipalityReportCount = await statisticsRepository.GetMunicipalityReportCountByIdAsync(neighborhood),
            };
        }
        catch (EntityNotFoundException)
        {
            if (track_request)
            {
                await analysisRepository.RegisterProductMismatch(id, tenantId);

                logger.LogInformation("{Name} requested product 'statistics' mismatch for identifier: {id}", organization.Name, id);
            }

            throw;
        }
    }
}
