using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Core.UseCases;
using FunderMaps.Helpers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for inquiry operations.
    /// </summary>
    [Route("inquiry")]
    public class InquiryController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly InquiryUseCase _inquiryUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryController(IMapper mapper, InquiryUseCase inquiryUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _inquiryUseCase = inquiryUseCase ?? throw new ArgumentNullException(nameof(inquiryUseCase));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var inquiry = await _inquiryUseCase.GetAsync(id).ConfigureAwait(false);

            return Ok(_mapper.Map<InquiryDto>(inquiry));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            var result = await _mapper.MapAsync<IList<InquiryDto>, Inquiry>(_inquiryUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAsync([FromQuery] PaginationModel pagination)
        {
            var result = await _mapper.MapAsync<IList<InquiryDto>, Inquiry>(_inquiryUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] InquiryDto input)
        {
            var inquiry = await _inquiryUseCase.CreateAsync(_mapper.Map<Inquiry>(input)).ConfigureAwait(false);

            return Ok(_mapper.Map<InquiryDto>(inquiry));
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync(IFormFile input)
        {
            // TODO: Replace with validator?
            var virtualFile = new ApplicationFileWrapper(input, Constants.AllowedFileMimes);
            if (!virtualFile.IsValid)
            {
                throw new ArgumentException(); // TODO
            }

            await _inquiryUseCase.StoreDocumentAsync(virtualFile.File, input.OpenReadStream()).ConfigureAwait(false);

            return Ok(virtualFile.File);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] InquiryDto input)
        {
            var inquiry = _mapper.Map<Inquiry>(input);
            inquiry.Id = id;

            await _inquiryUseCase.UpdateAsync(inquiry).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPut("{id:int}/status_review")]
        public async Task<IActionResult> SetStatusReviewAsync(int id)
        {
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.PendingReview).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPut("{id:int}/status_rejected")]
        public async Task<IActionResult> SetStatusRejectedAsync(int id)
        {
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.Rejected).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPut("{id:int}/status_approved")]
        public async Task<IActionResult> SetStatusApprovedAsync(int id)
        {
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.Done).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _inquiryUseCase.DeleteAsync(id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
