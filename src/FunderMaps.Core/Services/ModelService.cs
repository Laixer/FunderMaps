using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

internal class ModelService(
    IGeocoderTranslation geocoderTranslation,
    IAnalysisRepository analysisRepository,
    IStatisticsRepository statisticsRepository,
    IOrganizationRepository organizationRepository,
    ILogger<ModelService> logger) : IModelService
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

    public async Task<StatisticsProduct> GetStatisticsAsync(string id)
    {
        var product = new StatisticsProduct()
        {
            FoundationTypeDistribution = await statisticsRepository.GetFoundationTypeDistributionByIdAsync(id),
            ConstructionYearDistribution = await statisticsRepository.GetConstructionYearDistributionByIdAsync(id),
            DataCollectedPercentage = await statisticsRepository.GetDataCollectedPercentageByIdAsync(id),
            FoundationRiskDistribution = await statisticsRepository.GetFoundationRiskDistributionByIdAsync(id),
            TotalBuildingRestoredCount = await statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(id),
            TotalIncidentCount = await statisticsRepository.GetTotalIncidentCountByIdAsync(id),
            MunicipalityIncidentCount = await statisticsRepository.GetMunicipalityIncidentCountByIdAsync(id),
            TotalReportCount = await statisticsRepository.GetTotalReportCountByIdAsync(id),
            MunicipalityReportCount = await statisticsRepository.GetMunicipalityReportCountByIdAsync(id),
        };

        return product;
    }
}
