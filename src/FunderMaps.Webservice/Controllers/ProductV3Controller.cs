using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("v3/product")]
public class ProductV3Controller : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ProductV3Controller(IProductService productService)
        => _productService = productService ?? throw new ArgumentNullException(nameof(productService));

    // GET: api/v3/product/analysis
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    [HttpGet("analysis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<AnalysisProduct3> GetAnalysisAsync([FromQuery] string id)
        => _productService.GetAnalysis3Async(id);

    // GET: api/v3/product/at_risk
    /// <summary>
    ///     Request the risk index per id.
    /// </summary>
    [HttpGet("at_risk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<bool> GetRiskIndexAsync([FromQuery] string id)
        => _productService.GetRiskIndexAsync(id);

    // GET: api/v3/product/statistics
    /// <summary>
    ///     Request the statistics product.
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<StatisticsProduct> GetStatisticsAsync([FromQuery][Required] string id)
        => _productService.GetStatistics3Async(id);
}
