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
        private readonly Core.AppContext _appContext;
        private readonly ProductHandler _productHandler;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisController(Core.AppContext appContext, ProductHandler productHandler)
        {
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _productHandler = productHandler ?? throw new ArgumentNullException(nameof(productHandler));
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
        ///     <paramref name="input"/> is validated through <see cref="ApiControllerAttribute"/>.
        /// </remarks>
        /// <param name="input"><see cref="AnalysisInputModel"/></param>
        /// <param name="productRequestHandler"><see cref="ProductHandler"/></param>
        /// <returns><see cref="ResponseWrapper{TResponseModel}"/></returns>
        [HttpGet("get")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseWrapper<AnalysisResponseModelBase>))]
        public async Task<IActionResult> GetProductAsync([FromQuery] AnalysisInputModel input)
        {
            // Act.
            var result = await _productHandler.ProcessAnalysisRequestAsync(_appContext.UserId, input);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
