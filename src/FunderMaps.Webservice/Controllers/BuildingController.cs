using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Exceptions;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    /// Controller for all building endpoints.
    /// </summary>
    [Route("api/building")]
    [ApiController]
    public sealed class BuildingController : ControllerBase
    {
        private readonly ILogger<BuildingController> _logger;
        private readonly IProductResultService _productResultService;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public BuildingController(ILogger<BuildingController> logger,
            IProductResultService productResultService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productResultService = productResultService ?? throw new ArgumentNullException(nameof(productResultService));
        }

        /// <summary>
        /// Gets one or more <see cref="ResponseModelBase"/> items in a wrapper.
        /// TODO Improve doc
        /// TODO Do we split this to single and multiple?
        /// </summary>
        /// <param name="q"></param>
        /// <param name="product"></param>
        /// <param name="id"></param>
        /// <param name="bagid"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> GetProductAsync([FromRoute] string product, [FromRoute] string q , [FromRoute] string id, [FromRoute] string bagid, [FromRoute] uint? limit)
        {
            // Check for product
            if (string.IsNullOrEmpty(product))
            {
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = "Please specify a product"
                });
            }

            // Check for both bagid and id
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(bagid))
            {
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = "Can't specify both id and bagid at the same time"
                });
            }

            // Check for query in combination with id or bagid
            if (string.IsNullOrEmpty(q) && (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(bagid))) 
            {
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = "Can't specify a id or bagid when using a query"
                });
            }

            try
            {
                var funderMapsProduct = ProductTypeMapper.Map(product);
                var userId = Guid.NewGuid(); // TODO Implement auth

                // Process according to specified parameters
                // TODO Assignment, how to do more elegant?
                var response = null as ResponseWrapper<BuildingResponseModelBase>;
                if (!string.IsNullOrEmpty(q))
                {
                    response = await _productResultService.GetBuildingByQueryAsync(userId, funderMapsProduct, q, limit).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(id))
                {
                    response = await _productResultService.GetBuildingByIdAsync(userId, funderMapsProduct, id).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(bagid))
                {
                    response = await _productResultService.GetBuildingByBagidAsync(userId, funderMapsProduct, bagid).ConfigureAwait(false);
                }
                else
                {
                    // If we reach this point, we can't process the request
                    return BadRequest(new ErrorResponseModel
                    {
                        // TODO Implement error codes
                        InternalErrorCode = 1337,
                        Message = $"Could not parse request"
                    });
                }

                return Ok(response);
            }
            catch (ProductNotFoundException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = $"Could not parse product {product}"
                });
            }
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> GetProductAllAsync([FromRoute] string product)
        {
            // Check for product
            if (string.IsNullOrEmpty(product))
            {
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = "Please specify a product"
                });
            }

            try
            {
                var funderMapsProduct = ProductTypeMapper.Map(product);
                var userId = Guid.NewGuid(); // TODO Implement auth

                return Ok(await _productResultService.GetBuildingAllAsync(userId, funderMapsProduct).ConfigureAwait(false));
            }
            catch (ProductNotFoundException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new ErrorResponseModel
                {
                    // TODO Implement error codes
                    InternalErrorCode = 1337,
                    Message = $"Could not parse product {product}"
                });
            }
        }
    }
}
