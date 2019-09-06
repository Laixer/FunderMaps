using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers
{
    /// <summary>
    /// Base controller for all API controllers.
    /// </summary>
    [Controller]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Return unauthorized with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Unauthorized(int code, string message) => StatusCode(401, new ErrorOutputModel(code, message));

        /// <summary>
        /// Return bad request with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult BadRequest(int code, string message) => StatusCode(400, new ErrorOutputModel(code, message));

        /// <summary>
        /// Return bad request with an existing error model.
        /// </summary>
        /// <param name="model">Error model.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult BadRequest(ErrorOutputModel model) => StatusCode(400, model);

        /// <summary>
        /// Return not found with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult NotFound(int code, string message) => StatusCode(404, new ErrorOutputModel(code, message));

        /// <summary>
        /// Return default error model with resource not found.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceNotFound() => NotFound(0, "Resource not found");

        /// <summary>
        /// Return conflict with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Conflict(int code, string message) => StatusCode(409, new ErrorOutputModel(code, message));

        /// <summary>
        /// Return default error model with resource already exists.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceExists() => Conflict(0, "Resource already exists");

        /// <summary>
        /// Return forbidden with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult Forbid(int code, string message) => StatusCode(403, new ErrorOutputModel(code, message));

        /// <summary>
        /// Return default error model with resource already exists.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceForbid() => Forbid(0, "Resource access forbidden with current principal");

        /// <summary>
        /// Return application error with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult ApplicationError(int code = 0, string message = "Something went wrong during request processing")
            => StatusCode(500, new ErrorOutputModel(code, message));
    }
}