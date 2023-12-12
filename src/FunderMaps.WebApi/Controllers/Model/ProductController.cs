using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Model;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/product")]
public sealed class ProductController(IModelService modelService) : ControllerBase
{
    // GET: api/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [HttpGet("analysis/{id}")]
    public async Task<AnalysisProduct> GetAnalysisAsync(string id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        return await modelService.GetAnalysisAsync(id, tenantId);
    }

    // GET: api/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    [HttpGet("at_risk/{id}")]
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        return await modelService.GetRiskIndexAsync(id, tenantId);
    }

    // GET: api/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => modelService.GetStatisticsAsync(id);
}
