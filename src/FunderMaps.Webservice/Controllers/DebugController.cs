using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#if DEBUG
namespace FunderMaps.Webservice.Controllers
{
    [Route("debug")]
    public class DebugController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test() => Ok();

        [HttpGet("throw")]
        public IActionResult Throw() => throw new InvalidOperationException("This is my IOE");

        [HttpGet("throw_fm")]
        public IActionResult ThrowFm() => throw new FunderMapsCoreException("This is my FM exception");

        [HttpGet("throw_fm_pnfe")]
        public IActionResult ThrowFmPnfe() => throw new ProductNotFoundException("Could not find product");
    }
}
#endif
