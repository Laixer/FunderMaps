using System.Diagnostics;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Error()
        {
            return StatusCode(500, new ErrorOutoutModel
            {
                Title = "An error has occured on the remote side",
                Status = 500,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}