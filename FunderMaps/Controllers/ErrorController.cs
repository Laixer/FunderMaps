using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers
{
    public class ErrorController : Controller
    {
        public sealed class ErrorOutoutModel
        {
            public string Title { get; set; }
            public int Status { get; set; }
            public string TraceId { get; set; }
        }

        // GET: oops
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