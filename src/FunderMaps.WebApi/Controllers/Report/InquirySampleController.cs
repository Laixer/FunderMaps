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
    /// Endpoint controller for inquiry sample operations.
    /// </summary>
    [Route("inquiry/{inquiryId}/sample")]
    public class InquirySampleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly InquiryUseCase _inquiryUseCase;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public InquirySampleController(IMapper mapper, InquiryUseCase inquiryUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _inquiryUseCase = inquiryUseCase ?? throw new ArgumentNullException(nameof(inquiryUseCase));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var inquirySample = await _inquiryUseCase.GetSampleAsync(id);

            return Ok(_mapper.Map<InquirySampleDto>(inquirySample));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<InquirySampleDto>, InquirySample>(_inquiryUseCase.GetAllSampleAsync(pagination.Navigation));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(int inquiryId, [FromBody] InquirySampleDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inquirySample = _mapper.Map<InquirySample>(input);
            inquirySample.Inquiry = inquiryId;

            inquirySample = await _inquiryUseCase.CreateSampleAsync(inquirySample);

            return Ok(_mapper.Map<InquirySampleDto>(inquirySample));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int inquiryId, int id, [FromBody] InquirySampleDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inquirySample = _mapper.Map<InquirySample>(input);
            inquirySample.Id = id;
            inquirySample.Inquiry = inquiryId;

            await _inquiryUseCase.UpdateSampleAsync(inquirySample);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _inquiryUseCase.DeleteSampleAsync(id);

            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
