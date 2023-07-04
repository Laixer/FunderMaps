using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("v3/product")]
public class ProductController : ControllerBase
{
    private readonly IAnalysisRepository _analysisRepository;
    private readonly IStatisticsRepository _statisticsRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ProductController(
        IAnalysisRepository analysisRepository,
        IStatisticsRepository statisticsRepository)
    {
        _analysisRepository = analysisRepository;
        _statisticsRepository = statisticsRepository;
    }

    private async Task<StatisticsProduct> GetStatisticsByIdAsync(string id)
        => new()
        {
            FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByIdAsync(id),
            ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByIdAsync(id),
            DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByIdAsync(id),
            FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByIdAsync(id),
            TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(id),
            TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByIdAsync(id),
            MunicipalityIncidentCount = await _statisticsRepository.GetMunicipalityIncidentCountByIdAsync(id),
            TotalReportCount = await _statisticsRepository.GetTotalReportCountByIdAsync(id),
            MunicipalityReportCount = await _statisticsRepository.GetMunicipalityReportCountByIdAsync(id),
        };

    // TODO: LEGACY
    // GET: api/v3/product/analysis
    [HttpGet("analysis")]
    public async Task<IActionResult> GetAnalysisLegacyAsync([FromQuery] string id)
    {
        var product = await _analysisRepository.GetAsync(id);
        if (product is not null)
        {
            return Ok(product);
        }

        return NotFound();
    }

    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    // [Authorize(Roles = "webservice-analysis")]
    [HttpGet("analysis/{id}")]
    public async Task<IActionResult> GetAnalysisAsync(string id)
    {
        var product = await _analysisRepository.GetAsync(id);
        if (product is not null)
        {
            return Ok(product);
        }

        return NotFound();
    }

    // TODO: LEGACY
    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    // [Authorize(Roles = "webservice-statistics")]
    [HttpGet("at_risk")]
    public Task<bool> GetRiskIndexLegacyAsync([FromQuery] string id)
        => _analysisRepository.GetRiskIndexAsync(id);

    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    // [Authorize(Roles = "webservice-atrisk")]
    [HttpGet("at_risk/{id}")]
    public Task<bool> GetRiskIndexAsync(string id)
        => _analysisRepository.GetRiskIndexAsync(id);

    // TODO: LEGACY
    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    // [Authorize(Roles = "webservice-statistics")]
    [HttpGet("statistics")]
    public Task<StatisticsProduct> GetStatisticsLegacyAsync([FromQuery] string id)
        => GetStatisticsByIdAsync(id);

    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    // [Authorize(Roles = "webservice-statistics")]
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => GetStatisticsByIdAsync(id);
}
