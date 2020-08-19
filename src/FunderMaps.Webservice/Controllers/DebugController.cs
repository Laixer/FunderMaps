using AutoMapper;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    [Route("debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserTrackingService _userTrackingService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DebugController(IMapper mapper, IUserTrackingService userTrackingService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userTrackingService = userTrackingService ?? throw new ArgumentNullException(nameof(userTrackingService));
        }

        [HttpGet("test")]
        public IActionResult Test() => Ok();

        [HttpGet("throw")]
        public IActionResult Throw() => throw new InvalidOperationException("This is my IOE");

        [HttpGet("throw_fm")]
        public IActionResult ThrowFm() => throw new FunderMapsCoreException("This is my FM exception");

        [HttpGet("throw_fm_pnfe")]
        public IActionResult ThrowFmPnfe() => throw new ProductNotFoundException("Could not find product");

        [HttpGet("tracktest")]
        public async Task<IActionResult> TrackTestAsync(Guid userId)
        {
            await _userTrackingService.ProcessSingleAnalysisRequest(userId, AnalysisProductType.Complete, HttpContext.RequestAborted).ConfigureAwait(false);
            await _userTrackingService.ProcessMultipleAnalysisRequest(userId, AnalysisProductType.FoundationPlus, 14, HttpContext.RequestAborted).ConfigureAwait(false);
            await _userTrackingService.ProcessStatisticsRequestAsync(userId, StatisticsProductType.FoundationRatio, HttpContext.RequestAborted).ConfigureAwait(false);
            return Ok();
        }
    }
}
