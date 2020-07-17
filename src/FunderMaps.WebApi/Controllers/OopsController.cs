using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunderMaps.WebApi.Controllers
{
    /// <summary>
    ///     API error handler.
    /// </summary>
    public class OopsController : ControllerBase
    {
        // TODO: Maybe log the exception?

        /// <summary>
        ///     Wrap exceptions in a response.
        /// </summary>
        [Route("oops")]
        public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment, ILogger logger)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                logger.LogWarning("This should not be invoked in development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            if (exception is RepositoryException) // TODO: Should be removed.
            {
                return Problem(
                    statusCode: 404,
                    title: "Entity was not found.");
            }
            else if (exception is EntityNotFoundException)
            {
                return Problem(
                    statusCode: 404,
                    title: "Entity was not found.");
            }

            return Problem(
                statusCode: 500,
                title: "Application was unable to process request.");
        }
    }
}
