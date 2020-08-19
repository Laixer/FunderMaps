using AutoMapper;
using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FunderMaps.Webservice.Controllers
{
    [Route("debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IMapper _mapper;

        public DebugController(IMapper mapper) => _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

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
