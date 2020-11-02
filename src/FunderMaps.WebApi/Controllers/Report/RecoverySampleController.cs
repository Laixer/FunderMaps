using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
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
        private readonly RecoveryUseCase _recoveryUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoverySampleController(IMapper mapper, RecoveryUseCase recoveryUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _recoveryUseCase = recoveryUseCase ?? throw new ArgumentNullException(nameof(recoveryUseCase));
        }

        // GET: api/recovery/{id}/sample
        /// <summary>
        ///     Return recovery sample by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var recoverySample = await _recoveryUseCase.GetSampleAsync(id);

            return Ok(_mapper.Map<RecoverySampleDto>(recoverySample));
        }

        // GET: api/recovery/{id}/sample
        /// <summary>
        ///     Return all recovery samples.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoverySampleDto>, RecoverySample>(_recoveryUseCase.GetAllSampleAsync(pagination.Navigation));

            return Ok(result);
        }

        // POST: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Create recovery sample.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(int recoveryId, [FromBody] RecoverySampleDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recoverySample = _mapper.Map<RecoverySample>(input);
            recoverySample.Recovery = recoveryId;

            recoverySample = await _recoveryUseCase.CreateSampleAsync(recoverySample);

            return Ok(_mapper.Map<RecoverySampleDto>(recoverySample));
        }

        // PUT: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Update recovery sample by id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int recoveryId, int id, [FromBody] RecoverySampleDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recoverySample = _mapper.Map<RecoverySample>(input);
            recoverySample.Id = id;
            recoverySample.Recovery = recoveryId;

            await _recoveryUseCase.UpdateSampleAsync(recoverySample);

            return NoContent();
        }

        // DELETE: api/recovery/{id}/sample/{id}
        /// <summary>
        ///     Delete recovery sample by id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _recoveryUseCase.DeleteSampleAsync(id);

            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
