using Microsoft.AspNetCore.Mvc;
using FunderMaps.Models;

namespace FunderMaps.Controllers
{
    [Controller]
    public abstract class AbstractMicroController : ControllerBase
    {
        /// <summary>
        /// Return unauthorized with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Unauthorized(int code, string message)
        {
            return StatusCode(401, new ErrorOutputModel(code, message));
        }

        /// <summary>
        /// Return bad request with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult BadRequest(int code, string message)
        {
            return StatusCode(400, new ErrorOutputModel(code, message));
        }

        /// <summary>
        /// Return not found with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult NotFound(int code, string message)
        {
            return StatusCode(404, new ErrorOutputModel(code, message));
        }

        /// <summary>
        /// Return default error model with resource not found.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceNotFound()
        {
            return NotFound(0, "Resource not found");
        }

        /// <summary>
        /// Return conflict with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Conflict(int code, string message)
        {
            return StatusCode(409, new ErrorOutputModel(code, message));
        }

        /// <summary>
        /// Return default error model with resource already exists.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceExists()
        {
            return Conflict(0, "Resource already exists");
        }

        /// <summary>
        /// Return forbidden with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Forbid(int code, string message)
        {
            return StatusCode(403, new ErrorOutputModel(code, message));
        }

        /// <summary>
        /// Return default error model with resource already exists.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceForbid()
        {
            return Forbid(0, "Resource access forbidden with current principal");
        }
    }
}