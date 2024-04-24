using FunderMaps.Core.Controllers;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Model;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/product")]
public sealed class ProductController(ModelService modelService) : FunderMapsController
{
    // GET: api/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [HttpGet("analysis/{id}")]
    public Task<AnalysisProduct> GetAnalysisAsync(string id)
        => modelService.GetAnalysisAsync(id, TenantId);

    // GET: api/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => modelService.GetStatisticsAsync(id, TenantId);
}
