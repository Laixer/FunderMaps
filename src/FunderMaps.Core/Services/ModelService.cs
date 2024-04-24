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
    public async Task<AnalysisProduct> GetAnalysisAsync(string id, Guid tenantId, bool tracker_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var product = await analysisRepository.GetAsync(building.Id);

            if (tracker_request)
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
            if (tracker_request)
            {
                await analysisRepository.RegisterProductMismatch(id, tenantId);

                logger.LogInformation("{Name} requested product 'analysis3' mismatch for identifier: {id}", organization.Name, id);
            }

            throw;
        }
    }

    /// <summary>
    ///   Get risk index product.
    /// </summary>
    public async Task<bool> GetRiskIndexAsync(string id, Guid tenantId, bool tracker_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var product = await analysisRepository.GetRiskIndexAsync(building.Id);

            if (tracker_request)
            {
                var registered = await analysisRepository.RegisterProductMatch(building.Id, id, "riskindex", tenantId);
                if (registered)
                {
                    logger.LogInformation("{Name} registered 'riskindex' match for identifier: {id}", organization.Name, id);
                }
                else
                {
                    logger.LogInformation("{Name} retrieved 'riskindex' match for identifier: {id}", organization.Name, id);
                }
            }

            return product;
        }
        catch (EntityNotFoundException)
        {
            if (tracker_request)
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
    public async Task<StatisticsProduct> GetStatisticsAsync(string id, Guid tenantId, bool tracker_request = true)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var neighborhood = await geocoderTranslation.GetNeighborhoodIdAsync(id);

            var product = new StatisticsProduct()
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

            return product;

        }
        catch (EntityNotFoundException)
        {
            if (tracker_request)
            {
                await analysisRepository.RegisterProductMismatch(id, tenantId);

                logger.LogInformation("{Name} requested product 'statistics' mismatch for identifier: {id}", organization.Name, id);
            }

            throw;
        }
    }
}
