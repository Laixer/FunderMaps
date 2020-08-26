using FunderMaps.Webservice.Handlers;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all analysis endpoints.
    /// </summary>
    [Route("analysis")]
    public sealed class AnalysisController : ControllerBase
    {
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
        ///     <paramref name="input"/> is validated through <see cref="ApiControllerAttribute"/>.
        /// </remarks>
        /// <param name="input"><see cref="AnalysisInputModel"/></param>
        /// <param name="productRequestHandler"><see cref="ProductRequestHandler"/></param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseWrapper<AnalysisResponseModelBase>))]
        public async Task<IActionResult> GetProductAsync([FromQuery] AnalysisInputModel input, [FromServices] ProductRequestHandler productRequestHandler)
        {
            // Get user id.
            // TODO Implement auth
            var userId = Guid.NewGuid();

            var result = await productRequestHandler.ProcessAnalysisRequestAsync(userId, input, HttpContext.RequestAborted);

            // Process request and return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
