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

namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    /// Endpoint controller for inquiry operations.
    /// </summary>
    [ApiController]
    [Route("api/inquiry")]
    public class InquiryController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly InquiryUseCase _inquiryUseCase;

        /// <summary>
        /// Create new instance.
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

            return Ok(_mapper.Map<InquiryDTO>(inquiry));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            // FUTURE: Missing IAsyncEnum map()
            var result = new List<InquiryDTO>();
            await foreach (var item in _inquiryUseCase.GetAllAsync(pagination.Navigation))
            {
                result.Add(_mapper.Map<InquiryDTO>(item));
            }

            //var result = _mapper.Map<IAsyncEnumerable<Inquiry>, List<InquiryDTO>>(_inquiryUseCase.GetAllInquiryAsync(pagination.Navigation));

            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            // FUTURE: Missing IAsyncEnum map()
            var result = new List<InquiryDTO>();
            await foreach (var item in _inquiryUseCase.GetAllAsync(pagination.Navigation))
            {
                result.Add(_mapper.Map<InquiryDTO>(item));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] InquiryDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inquiry = await _inquiryUseCase.CreateAsync(_mapper.Map<Inquiry>(input)).ConfigureAwait(false);

            return Ok(_mapper.Map<InquiryDTO>(inquiry));
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync(IFormFile input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // TODO: Replace with validator?
            var virtualFile = new ApplicationFileWrapper(input, Constants.AllowedFileMimes);
            if (!virtualFile.IsValid)
            {
                throw new ArgumentException();
            }

            await _inquiryUseCase.StoreDocumentAsync(virtualFile.File, input.OpenReadStream()).ConfigureAwait(false);

            return Ok(virtualFile.File);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] InquiryDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

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
