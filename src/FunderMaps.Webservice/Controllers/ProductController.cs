using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/v3/product")]
public sealed class ProductController(
    IAnalysisRepository analysisRepository,
    IStatisticsRepository statisticsRepository,
    IGeocoderTranslation geocoderTranslation,
    IOrganizationRepository organizationRepository,
    ILogger<ProductController> logger) : ControllerBase
{
    private async Task<StatisticsProduct> GetStatisticsByIdAsync(string id)
        => new()
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

    // TODO: LEGACY
    // GET: api/v3/product/analysis
    [HttpGet("analysis")]
    public Task<AnalysisProduct> GetAnalysisLegacyAsync([FromQuery] string id)
        => GetAnalysisAsync(id);

    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [HttpGet("analysis/{id}")]
    public async Task<AnalysisProduct> GetAnalysisAsync(string id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());
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

            HttpContext.Response.Headers.Append("X-FunderMaps-Product-Registered", registered ? "1" : "0");

            return product;
        }
        catch (EntityNotFoundException)
        {
            await analysisRepository.RegisterProductMismatch(id, tenantId);

            logger.LogInformation("{Name} requested product 'analysis3' mismatch for identifier: {id}", organization.Name, id);

            throw;
        }
    }

    // TODO: LEGACY
    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    [HttpGet("at_risk")]
    public Task<bool> GetRiskIndexLegacyAsync([FromQuery] string id)
        => GetRiskIndexAsync(id);

    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    [HttpGet("at_risk/{id}")]
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());
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

            HttpContext.Response.Headers.Append("X-FunderMaps-Product-Registered", registered ? "1" : "0");

            return product;
        }
        catch (EntityNotFoundException)
        {
            await analysisRepository.RegisterProductMismatch(id, tenantId);

            logger.LogInformation("{Name} requested product 'riskindex' mismatch for identifier: {id}", organization.Name, id);

            throw;
        }
    }

    // TODO: LEGACY
    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [HttpGet("statistics")]
    public Task<StatisticsProduct> GetStatisticsLegacyAsync([FromQuery] string id)
        => GetStatisticsAsync(id);

    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => GetStatisticsByIdAsync(id);
}
