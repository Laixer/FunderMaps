﻿using FunderMaps.Core.Controllers;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/v3/product")]
public sealed class ProductController(ModelService modelService) : FunderMapsController
{
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
        return await modelService.GetAnalysisAsync(id, TenantId);
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
        return await modelService.GetRiskIndexAsync(id, TenantId);
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
    {
        return modelService.GetStatisticsAsync(id, TenantId);
    }
}
