using FunderMaps.Core.Exceptions;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    /// Controller for all statistics endpoints.
    /// </summary>
    [ApiController, Route("api/statistics")]
    public sealed class StatisticsController : ControllerBase
    {
        private const uint DefaultPage = 1;
        private const uint DefaultLimit = 25;
        private readonly ILogger<StatisticsController> _logger;
        private readonly IProductResultService _productResultService;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public StatisticsController(ILogger<StatisticsController> logger,
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
        /// <param name="areaCode">Get all buildings in an area code</param>
        /// <param name="fullFence">Get all buildings in the geofence</param>
        /// <param name="page">Page to display</param>
        /// <param name="limit">Items per page</param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetProductAsync(string product, string areaCode, bool fullFence, uint? page, uint? limit)
        {
            // Check for product.
            if (string.IsNullOrEmpty(product))
            {
                return BadRequest(Problem("Please specify a product"));
            }

            // Check for invalid combinations.
            if ((fullFence && !string.IsNullOrEmpty(areaCode)) ||
                (!fullFence && string.IsNullOrEmpty(areaCode)))
            {
                return Problem("Please select one of the following options: area code or fullfence");
            }

            try
            {
                var analysisProduct = ProductTypeMapper.MapStatisticsFromString(product);
                var userId = Guid.NewGuid(); // TODO Implement auth

                // Process according to specified parameters
                // TODO Assignment, how to do more elegant?
                var response = null as ResponseWrapper;
                if (!string.IsNullOrEmpty(areaCode))
                {
                    response = await _productResultService.GetStatisticsByAreaAsync(userId, analysisProduct, areaCode, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else if (fullFence)
                {
                    response = await _productResultService.GetStatisticsInFenceAsync(userId, analysisProduct, page ?? DefaultPage, limit ?? DefaultLimit).ConfigureAwait(false);
                }
                else
                {
                    // If we reach this point, we can't process the request.
                    return Problem($"Could not parse request");
                }

                return Ok(response);
            }
            // TODO Use core exception? I think so, yes.
            // TODO Use error handling according to issue #167
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