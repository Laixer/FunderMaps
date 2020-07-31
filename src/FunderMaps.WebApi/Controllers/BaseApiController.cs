using FunderMaps.WebApi.ViewModels;
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
        /// Return not found with an error model.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult NotFound(int code, string message) =>
            StatusCode(404, message);

        /// <summary>
        /// Return default error model with resource not found.
        /// </summary>
        /// <returns>ActionResult.</returns>
        protected ActionResult ResourceNotFound() =>
            NotFound(0, "Resource not found");
    }
}