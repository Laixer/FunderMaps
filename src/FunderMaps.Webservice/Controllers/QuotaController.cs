using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all product quotas.
    /// </summary>
    [Route("quota")]
    public sealed class QuotaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITelemetryRepository _telemetryRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QuotaController(IMapper mapper, ITelemetryRepository telemetryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _telemetryRepository = telemetryRepository ?? throw new ArgumentNullException(nameof(telemetryRepository));
        }

        // GET: api/quota/usage
        /// <summary>
        ///     Request product quota usage.
        /// </summary>
        [HttpGet("usage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseWrapper<StatisticsDto>>> GetQuotaUsageAsync()
        {
            // Assign.
            IAsyncEnumerable<ProductTelemetry> productUsageList = _telemetryRepository.ListAllUsageAsync();

            // Map.
            var result = await _mapper.MapAsync<IList<ProductTelemetry>, ProductTelemetry>(productUsageList);

            // Return.
            return Ok(productUsageList);
        }
    }
}
