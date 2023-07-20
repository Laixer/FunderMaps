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
public class ProductController : ControllerBase
{
    private readonly Core.AppContext _appContext;
    private readonly IAnalysisRepository _analysisRepository;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IGeocoderTranslation _geocoderTranslation;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ILogger<ProductController> _logger;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ProductController(
        Core.AppContext appContext,
        IAnalysisRepository analysisRepository,
        IStatisticsRepository statisticsRepository,
        IGeocoderTranslation geocoderTranslation,
        IOrganizationRepository organizationRepository,
        ILogger<ProductController> logger)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _analysisRepository = analysisRepository;
        _statisticsRepository = statisticsRepository;
        _geocoderTranslation = geocoderTranslation;
        _organizationRepository = organizationRepository;
        _logger = logger;
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
    public async Task<AnalysisProduct> GetAnalysisLegacyAsync([FromQuery] string id)
        => await GetAnalysisAsync(id);

    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [HttpGet("analysis/{id}")]
    public async Task<AnalysisProduct> GetAnalysisAsync(string id)
    {
        var organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);

        try
        {
            var building = await _geocoderTranslation.GetBuildingIdAsync(id);
            var product = await _analysisRepository.GetAsync(building.Id);

            var registered = await _analysisRepository.RegisterProductMatch(building.Id, id, "analysis3");
            if (registered)
            {
                _logger.LogInformation($"{organization.Name} registered 'analysis3' match for identifier: {id}");
            }
            else
            {
                _logger.LogInformation($"{organization.Name} retrieved 'analysis3' match for identifier: {id}");
            }

            HttpContext.Response.Headers.Add("X-FunderMaps-Product-Registered", registered ? "1" : "0");

            return product;
        }
        catch (EntityNotFoundException)
        {
            await _analysisRepository.RegisterProductMismatch(id);

            _logger.LogInformation($"{organization.Name} requested product 'analysis3' mismatch for identifier: {id}");

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
        var organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);

        try
        {
            var building = await _geocoderTranslation.GetBuildingIdAsync(id);
            var product = await _analysisRepository.GetRiskIndexAsync(building.Id);

            var registered = await _analysisRepository.RegisterProductMatch(building.Id, id, "riskindex");
            if (registered)
            {
                _logger.LogInformation($"{organization.Name} registered 'riskindex' match for identifier: {id}");
            }
            else
            {
                _logger.LogInformation($"{organization.Name} retrieved 'riskindex' match for identifier: {id}");
            }

            HttpContext.Response.Headers.Add("X-FunderMaps-Product-Registered", registered ? "1" : "0");

            return product;
        }
        catch (EntityNotFoundException)
        {
            await _analysisRepository.RegisterProductMismatch(id);

            _logger.LogInformation($"{organization.Name} requested product 'riskindex' mismatch for identifier: {id}");

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
