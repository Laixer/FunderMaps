using FunderMaps.Core.Controllers;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/v3/product")]
public sealed class ProductController(ModelService modelService) : FunderMapsController
{
    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("analysis/{id}")]
    public Task<AnalysisProduct> GetAnalysisAsync(string id)
        => modelService.GetAnalysisAsync(id, TenantId);

    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("at_risk/{id}")]
    public Task<bool> GetRiskIndexAsync(string id)
        => modelService.GetRiskIndexAsync(id, TenantId);

    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => modelService.GetStatisticsAsync(id, TenantId);
}
