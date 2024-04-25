﻿using FunderMaps.Core.Controllers;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("api/v3/product")]
public sealed class ProductController(ModelService modelService, ILogger<ProductController> logger) : FunderMapsController
{
    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("analysis/{id}")]
    public Task<AnalysisProduct> GetAnalysisAsync(string id)
        => modelService.GetAnalysisAsync(id, TenantId);

    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("statistics/{id}")]
    public Task<StatisticsProduct> GetStatisticsAsync(string id)
        => modelService.GetStatisticsAsync(id, TenantId);

    // TODO: LEGACY
    // GET: api/v3/product/analysis
    [Authorize(Roles = "Service, Administrator")]
    [HttpGet("analysis")]
    public Task<AnalysisProduct> GetAnalysisLegacyAsync([FromQuery] string id)
    {
        logger.LogWarning("Legacy endpoint called: {Endpoint}", HttpContext.Request.Path);

        return GetAnalysisAsync(id);
    }

    // TODO: LEGACY
    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [Authorize(Roles = "Service,Administrator")]
    [HttpGet("statistics")]
    public Task<StatisticsProduct> GetStatisticsLegacyAsync([FromQuery] string id)
    {
        logger.LogWarning("Legacy endpoint called: {Endpoint}", HttpContext.Request.Path);

        return GetStatisticsAsync(id);
    }
}
