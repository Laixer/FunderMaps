using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FunderMaps.Controllers
{
    /// <summary>
    /// API error handler.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Output model for error.
        /// </summary>
        internal sealed class ServerErrorOutoutModel
        {
            /// <summary>
            /// Descriptive error message.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// HTTP status code.
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// Request identifier,
            /// </summary>
            public string TraceId { get; set; }
        }

        // GET: oops
        /// <summary>
        /// Return a server error to the client.
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServerErrorOutoutModel), 500)]
        public IActionResult Error()
        {
            return StatusCode(500, new ServerErrorOutoutModel
            {
                Title = "An error has occured on the remote side",
                Status = 500,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}