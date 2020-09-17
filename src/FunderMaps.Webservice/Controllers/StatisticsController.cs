using FunderMaps.Core.Authentication;
using FunderMaps.Webservice.Handlers;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all statistics endpoints.
    /// </summary>
    [Route("statistics")]
    public sealed class StatisticsController : ControllerBase
    {
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
        ///     <paramref name="input"/> is validated through <see cref="ApiControllerAttribute"/>.
        /// </remarks>
        /// <param name="input"><see cref="StatisticsInputModel"/></param>
        /// <param name="productRequestHandler"><see cref="ProductRequestHandler"/></param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetProductAsync([FromQuery] StatisticsInputModel input, 
            [FromServices] ProductRequestHandler productRequestHandler,
            [FromServices] AuthManager authManager)
        {
            // Get user id.
            var user = await authManager.GetUserAsync(User);

            // Process request and return.
            var result = await productRequestHandler.ProcessStatisticsRequestAsync(user.Id, input, HttpContext.RequestAborted);

            // Return in ok result.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
