using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     API error handler.
    /// </summary>
    [AllowAnonymous]
    public class OopsController : ControllerBase
    {
        // GET: oops
        /// <summary>
        ///     Returns a <see cref="ProblemDetails"/> based on the <see cref="IExceptionHandlerFeature"/>
        ///     which is present in the current <see cref="ControllerBase.HttpContext"/>.
        /// </summary>
        /// <returns>Instance of <see cref="ProblemDetails"/>.</returns>
        [Route("oops")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment, [FromServices] ILogger<OopsController> logger)
        {
            var error = HttpContext.Features.Get<ErrorMessage>();

            // If the error message is not set just return a generic problem.
            if (error is null)
            {
                logger.LogWarning($"Cannot return configured error message from exception, return generic problem");

                if (webHostEnvironment.IsDevelopment())
                {
                    logger.LogWarning("This should not be invoked in development environments.");
                }

                return Problem(
                    title: "Application was unable to process the request.",
                    statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Problem(
                title: error.Message,
                statusCode: error.StatusCode);
        }
    }
}
