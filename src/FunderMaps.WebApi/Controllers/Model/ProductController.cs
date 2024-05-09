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
    // GET: api/product/analysis/{id}
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    /// <remarks>
    ///     Cache response for 12 hours. Analysis will not change often.
    /// </remarks>
    [HttpGet("analysis/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<AnalysisProduct> GetAnalysisAsync(string id)
        => modelService.GetAnalysisAsync(id, TenantId);

    // GET: api/product/statistics/{id}
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    /// <remarks>
    ///     Cache response for 12 hours. Statistics will not change often.
    /// </remarks>
    [HttpGet("statistics/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => modelService.GetStatisticsAsync(id, TenantId);

    // GET: api/product/building/{id}
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    /// <remarks>
    ///     Cache response for 12 hours. Statistics will not change often.
    /// </remarks>
    [HttpGet("statistics/building/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<StatisticsProduct> GetStatistics2Async(string id)
        => modelService.GetStatistics2Async(id, TenantId);
}
