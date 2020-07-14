using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("FunderMaps.Tests")]

namespace FunderMaps.Controllers
{
    /// <summary>
    /// API error handler.
    /// </summary>
    public class ErrorController : Controller
    {
        // GET: oops
        /// <summary>
        /// Return a server error to the client.
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApplicationErrorModel), 500)]
        public IActionResult Error()
            => StatusCode(500, new ApplicationErrorModel
            {
                Title = "An error has occured on the remote side",
                Status = 500,
                TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
            });
    }
}