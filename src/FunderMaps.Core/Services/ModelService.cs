using FunderMaps.Core.Components;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

public class ModelService(
    GeocoderTranslation geocoderTranslation,
    IAnalysisRepository analysisRepository,
    IStatisticsRepository statisticsRepository,
    IOrganizationRepository organizationRepository,
    ILogger<ModelService> logger)
{
    public async Task<AnalysisProduct> GetAnalysisAsync(string id, Guid tenantId)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var product = await analysisRepository.GetAsync(building.Id);

            var registered = await analysisRepository.RegisterProductMatch(building.Id, id, "analysis3", tenantId);
            if (registered)
            {
                logger.LogInformation("{Name} registered 'analysis3' match for identifier: {id}", organization.Name, id);
            }
            else
            {
                logger.LogInformation("{Name} retrieved 'analysis3' match for identifier: {id}", organization.Name, id);
            }

            return product;
        }
        catch (EntityNotFoundException)
        {
            await analysisRepository.RegisterProductMismatch(id, tenantId);

            logger.LogInformation("{Name} requested product 'analysis3' mismatch for identifier: {id}", organization.Name, id);

            throw;
        }
    }

    public async Task<bool> GetRiskIndexAsync(string id, Guid tenantId)
    {
        var organization = await organizationRepository.GetByIdAsync(tenantId);

        try
        {
            var building = await geocoderTranslation.GetBuildingIdAsync(id);
            var product = await analysisRepository.GetRiskIndexAsync(building.Id);

            var registered = await analysisRepository.RegisterProductMatch(building.Id, id, "riskindex", tenantId);
            if (registered)
            {
                logger.LogInformation("{Name} registered 'riskindex' match for identifier: {id}", organization.Name, id);
            }
            else
            {
                logger.LogInformation("{Name} retrieved 'riskindex' match for identifier: {id}", organization.Name, id);
            }

            return product;
        }
        catch (EntityNotFoundException)
        {
            await analysisRepository.RegisterProductMismatch(id, tenantId);

            logger.LogInformation("{Name} requested product 'analysis3' mismatch for identifier: {id}", organization.Name, id);

            throw;
        }
    }

    public async Task<StatisticsProduct> GetStatisticsAsync(string id, Guid tenantId)
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
            await analysisRepository.RegisterProductMismatch(id, tenantId);

            logger.LogInformation("{Name} requested product 'statistics' mismatch for identifier: {id}", organization.Name, id);

            throw;
        }
    }
}
