using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for recovery operations.
    /// </summary>
    [ApiController]
    [Route("api/recovery")]
    public class RecoveryController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly RecoveryUseCase _recoveryUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryController(IMapper mapper, RecoveryUseCase recoveryUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _recoveryUseCase = recoveryUseCase ?? throw new ArgumentNullException(nameof(recoveryUseCase));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var recovery = await _recoveryUseCase.GetAsync(id).ConfigureAwait(false);

            return Ok(_mapper.Map<RecoveryDTO>(recovery));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoveryDTO>, Recovery>(_recoveryUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoveryDTO>, Recovery>(_recoveryUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecoveryDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recovery = await _recoveryUseCase.CreateAsync(_mapper.Map<Recovery>(input)).ConfigureAwait(false);

            return Ok(_mapper.Map<RecoveryDTO>(recovery));
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync(IFormFile input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // FUTURE

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RecoveryDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recovery = _mapper.Map<Recovery>(input);
            recovery.Id = id;

            await _recoveryUseCase.UpdateAsync(recovery).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> SetStatusAsync(int id, [FromBody] RecoveryDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var recovery = _mapper.Map<Recovery>(input);
            recovery.Id = id;

            // FUTURE

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _recoveryUseCase.DeleteAsync(id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
