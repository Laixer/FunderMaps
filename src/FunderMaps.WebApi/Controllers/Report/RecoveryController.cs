using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for recovery operations.
    /// </summary>
    [Route("recovery")]
    public class RecoveryController : ControllerBase
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
            // Act.
            var recovery = await _recoveryUseCase.GetAsync(id);

            // Map.
            var output = _mapper.Map<RecoveryDto>(recovery);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoveryDto>, Recovery>(_recoveryUseCase.GetAllAsync(pagination.Navigation));

            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAsync([FromQuery] PaginationDto pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<RecoveryDto>, Recovery>(_recoveryUseCase.GetAllAsync(pagination.Navigation));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecoveryDto input)
        {
            // Map.
            var recovery = _mapper.Map<Recovery>(input);

            // Act.
            recovery = await _recoveryUseCase.CreateAsync(recovery);

            // Map.
            var output = _mapper.Map<RecoveryDto>(recovery);

            // Return.
            return Ok(output);
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync([Required] IFormFile input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // FUTURE

            // Return.
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RecoveryDto input)
        {
            // Map.
            var recovery = _mapper.Map<Recovery>(input);
            recovery.Id = id;

            // Act.
            await _recoveryUseCase.UpdateAsync(recovery);

            // Return.
            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> SetStatusAsync(int id, [FromBody] RecoveryDto input)
        {
            // Map.
            var recovery = _mapper.Map<Recovery>(input);
            recovery.Id = id;

            // FUTURE

            // Return.
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            await _recoveryUseCase.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
