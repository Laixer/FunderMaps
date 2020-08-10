using FunderMaps.Core.Exceptions;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all statistics endpoints.
    /// </summary>
    [ApiController, Route("api/statistics")]
    public sealed class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IProductRequestService _productRequestService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StatisticsController(ILogger<StatisticsController> logger,
            IProductRequestService productRequestService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRequestService = productRequestService ?? throw new ArgumentNullException(nameof(productRequestService));
        }

        /// <summary>
        ///     Gets one or more <see cref="ResponseModelBase"/> items in a wrapper. 
        ///     Specify one of the following:
        /// <list type="bullet">
        ///     <item>- Query string <paramref name="q"/></item>
        ///     <item>- Internal id <paramref name="id"/></item>
        ///     <item>- BAG id <paramref name="bagid"/></item>
        ///     <item>- Get all buildings in geofence <paramref name="fullFence"/></item>
        /// </list>
        /// </summary>
        /// <remarks>
        ///     <paramref name="inputModel"/> is validated through <see cref="ApiControllerAttribute"/>.
        /// </remarks>
        /// <param name="inputModel"><see cref="StatisticsInputModel"/></param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetProductAsync([FromQuery] StatisticsInputModel inputModel)
        {
            // Get user id.
            // TODO Implement auth
            var userId = Guid.NewGuid();

            // Process request and return.
            return Ok(await _productRequestService.ProcessStatisticsRequestAsync(userId, inputModel, HttpContext.RequestAborted).ConfigureAwait(false));
        }
    }
}