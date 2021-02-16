using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for recovery sample operations.
    /// </summary>
    [Route("recovery/{recoveryId}/sample")]
    public class RecoverySampleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecoverySampleRepository _recoverySampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoverySampleController(
            IMapper mapper,
            IRecoverySampleRepository recoverySampleRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _recoverySampleRepository = recoverySampleRepository ?? throw new ArgumentNullException(nameof(recoverySampleRepository));
        }

        // GET: api/recovery/{id}/sample/stats
        /// <summary>
        ///     Return recovery report sample statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatsAsync(int recoveryId)
        {
            // Map.
            DatasetStatsDto output = new()
            {
                Count = await _recoverySampleRepository.CountAsync(recoveryId),
            };

            // Return.
            return Ok(output);
        }

        // GET: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Return recovery sample by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Act.
            RecoverySample recoverySample = await _recoverySampleRepository.GetByIdAsync(id);

            // Map.
            RecoverySampleDto output = _mapper.Map<RecoverySampleDto>(recoverySample);

            // Return.
            return Ok(output);
        }

        // GET: api/recovery/{id}/sample
        /// <summary>
        ///     Return all recovery samples.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int recoveryId, [FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<RecoverySample> recoverySampleList = _recoverySampleRepository.ListAllAsync(recoveryId, pagination.Navigation);

            // Map.
            IList<RecoverySampleDto> output = await _mapper.MapAsync<IList<RecoverySampleDto>, RecoverySample>(recoverySampleList);

            // Return.
            return Ok(output);
        }

        // POST: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Create recovery sample.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(int recoveryId, [FromBody] RecoverySampleDto input, [FromServices] IGeocoderTranslation geocoderTranslation)
        {
            Address address = await geocoderTranslation.GetAddressIdAsync(input.Address);

            // Map.
            RecoverySample recoverySample = _mapper.Map<RecoverySample>(input);
            recoverySample.Address = address.Id;
            recoverySample.Recovery = recoveryId;

            // Act.
            recoverySample = await _recoverySampleRepository.AddGetAsync(recoverySample);

            // Map.
            RecoverySampleDto output = _mapper.Map<RecoverySampleDto>(recoverySample);

            // Return.
            return Ok(output);
        }

        // PUT: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Update recovery sample by id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int recoveryId, int id, [FromBody] RecoverySampleDto input)
        {
            // Map.
            RecoverySample recoverySample = _mapper.Map<RecoverySample>(input);
            recoverySample.Id = id;
            recoverySample.Recovery = recoveryId;

            // Act.
            await _recoverySampleRepository.UpdateAsync(recoverySample);

            // Return.
            return NoContent();
        }

        // DELETE: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Delete recovery sample by id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            await _recoverySampleRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
