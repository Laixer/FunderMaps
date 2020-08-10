using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ApiController, Route("api/recovery/{recoveryId}/sample")]
    public class RecoverySampleController : BaseApiController
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var recoverySample = await _recoveryUseCase.GetSampleAsync(id).ConfigureAwait(false);

            return Ok(_mapper.Map<RecoverySampleDto>(recoverySample));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoverySampleDto>, RecoverySample>(_recoveryUseCase.GetAllSampleAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(int recoveryId, [FromBody] RecoverySampleDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recoverySample = _mapper.Map<RecoverySample>(input);
            recoverySample.Recovery = recoveryId;

            recoverySample = await _recoveryUseCase.CreateSampleAsync(recoverySample).ConfigureAwait(false);

            return Ok(_mapper.Map<RecoverySampleDto>(recoverySample));
        }

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

            await _recoveryUseCase.UpdateSampleAsync(recoverySample).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _recoveryUseCase.DeleteSampleAsync(id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
