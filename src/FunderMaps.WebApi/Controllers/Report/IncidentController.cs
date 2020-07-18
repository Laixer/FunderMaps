using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for incident operations.
    /// </summary>
    [ApiController]
    [Route("api/incident")]
    public class IncidentController : BaseApiController
    {
        private const string gatewayName = "FunderMaps.WebApi";

        private readonly IMapper _mapper;
        private readonly IncidentUseCase _incidentUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentController(IMapper mapper, IncidentUseCase incidentUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _incidentUseCase = incidentUseCase ?? throw new ArgumentNullException(nameof(incidentUseCase));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([Incident] string id)
        {
            var incident = await _incidentUseCase.GetAsync(id).ConfigureAwait(false);

            var output = _mapper.Map<IncidentDTO>(incident);

            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<IncidentDTO>, Incident>(_incidentUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] IncidentDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var incident = _mapper.Map<Incident>(input);
            incident.Meta = new
            {
                // FUTURE: Register org?
                Gateway = gatewayName,
            };

            incident = await _incidentUseCase.CreateAsync(incident).ConfigureAwait(false);

            return Ok(_mapper.Map<IncidentDTO>(incident));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([Incident] string id, [FromBody] IncidentDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var incident = _mapper.Map<Incident>(input);
            incident.Id = id;

            await _incidentUseCase.UpdateAsync(incident).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([Incident] string id)
        {
            await _incidentUseCase.DeleteAsync(id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
