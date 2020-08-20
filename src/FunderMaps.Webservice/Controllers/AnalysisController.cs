using FunderMaps.Core.Exceptions;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all analysis endpoints.
    /// </summary>
    [ApiController, Route("api/analysis")]
    public sealed class AnalysisController : ControllerBase
    {
        private readonly ILogger<AnalysisController> _logger;
        private readonly IProductRequestService _productRequestService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisController(ILogger<AnalysisController> logger,
            IProductRequestService productRequestService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRequestService = productRequestService ?? throw new ArgumentNullException(nameof(productRequestService));
        }

        /// <summary>
        ///     Gets one or more <see cref="ResponseModelBase"/> items in a wrapper. 
        ///     Specify one of the following:
        /// <list type="bullet">
        ///     <item>Query string <paramref name="q"/></item>
        ///     <item>Internal id <paramref name="id"/></item>
        ///     <item>BAG id <paramref name="bagid"/></item>
        ///     <item>Get all buildings in geofence <paramref name="fullFence"/></item>
        /// </list>
        /// </summary>
        /// <remarks>
        ///     <paramref name="inputModel"/> is validated through <see cref="ApiControllerAttribute"/>.
        /// </remarks>
        /// <param name="inputModel"><see cref="AnalysisInputModel"/></param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AnalysisResponseWrapper<AnalysisResponseModelBase>))]
        public async Task<IActionResult> GetProductAsync([FromQuery] AnalysisInputModel inputModel)
        {
            // Get user id.
            // TODO Implement auth
            var userId = Guid.NewGuid();

            // Process request and return.
            return Ok(await _productRequestService.ProcessAnalysisRequestAsync(userId, inputModel, HttpContext.RequestAborted).ConfigureAwait(false));
        }
    }
}
