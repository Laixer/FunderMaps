using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     API error handler.
/// </summary>
[AllowAnonymous]
public class OopsController : ControllerBase
{
    // GET: oops
    /// <summary>
    ///     Returns a <see cref="ProblemDetails"/> in case somthing went wrong.
    /// </summary>
    /// <remarks>
    ///     This controller should never be called. Any application exceptions should
    ///     be handled by the exception filter.
    /// </remarks>
    [Route("oops")]
    public IActionResult Error([FromServices] ILogger<OopsController> logger)
    {
        logger.LogWarning("Cannot return configured error message from exception, return generic problem");

        return Problem(
            title: "Application was unable to process the request.",
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
