using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    [Route("debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IMapper _mapper;

        public DebugController(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("test")]
        public IActionResult Test()
        {


            return Ok();
        }

    }
}
