using FunderMaps.Core.Exceptions;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    /// Controller for all analysis endpoints.
    /// </summary>
    [Route("api/analysis")]
    [ApiController]
    public sealed class AnalysisController : ControllerBase
    {
        private const uint DefaultPage = 1;
        private const uint DefaultLimit = 25;
        private readonly ILogger<AnalysisController> _logger;
        private readonly IProductResultService _productResultService;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AnalysisController(ILogger<AnalysisController> logger,
            IProductResultService productResultService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productResultService = productResultService ?? throw new ArgumentNullException(nameof(productResultService));
        }

        /// <summary>
        /// Gets one or more <see cref="ResponseModelBase"/> items in a wrapper. 
        /// Specify one of the following:
        /// - Query string <paramref name="q"/>
        /// - Internal id <paramref name="id"/>
        /// - BAG id <paramref name="bagid"/>
        /// - Get all buildings in geofence <paramref name="fullFence"/>
        /// </summary>
        /// <remarks>
        /// The default for <paramref name="page"/> is <see cref="DefaultPage"/>.
        /// The default for <paramref name="limit"/> is <see cref="DefaultLimit"/>./// 
        /// </remarks>
        /// <param name="product">The requested product type</param>
        /// <param name="q">Query string</param>
        /// <param name="id">Internal building id</param>
        /// <param name="bagid">BAG id</param>
        /// <param name="fullFence">Get all buildings in the geofence</param>
        /// <param name="page">Page to display</param>
        /// <param name="limit">Items per page</param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetProductAsync([FromRoute] string product, [FromRoute] string q,
            [FromRoute] string id, [FromRoute] string bagid, [FromRoute] bool fullFence, [FromRoute] uint? page, [FromRoute] uint? limit)
        {
            // Check for product
            if (string.IsNullOrEmpty(product))
            {
                return BadRequest(Problem("Please specify a product"));
            }

            // Check for invalid combinations
            // Check for both bagid and id
            if ((fullFence && ArgumentUtility.NotNullCount(q, id, bagid) > 0) ||
                (ArgumentUtility.NotNullCount(q, id, bagid) > 1) ||
                (!fullFence && ArgumentUtility.NotNullCount(q, id, bagid) == 0))
            {
                return Problem("Please select one of the following options: id, bagid, query or fullfence");
            }

            try
            {
                var analysisProduct = ProductTypeMapper.MapAnalysis(product);
                var userId = Guid.NewGuid(); // TODO Implement auth

                // Process according to specified parameters
                // TODO Assignment, how to do more elegant?
                var response = null as ResponseWrapper<AnalysisResponseModelBase>;
                if (!string.IsNullOrEmpty(q))
                {
                    response = await _productResultService.GetAnalysisByQueryAsync(userId, analysisProduct, q, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(id))
                {
                    response = await _productResultService.GetAnalysisByIdAsync(userId, analysisProduct, id, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(bagid))
                {
                    response = await _productResultService.GetAnalysisByBagIdAsync(userId, analysisProduct, bagid, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else if (fullFence)
                {
                    response = await _productResultService.GetAnalysisInFenceAsync(userId, analysisProduct, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else
                {
                    // If we reach this point, we can't process the request
                    return Problem($"Could not parse request");
                }

                return Ok(response);
            }
            // TODO Use core exception? I think so, yes.
            catch (ProductNotFoundException e)
            {
                _logger.LogError(e.Message);
                return Problem($"Could not parse product {product}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem("Something went wrong");
            }
        }
    }
}
