using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using FunderMaps.Core.UseCases;
using FunderMaps.Helpers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
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
    ///     Endpoint controller for inquiry operations.
    /// </summary>
    [Route("inquiry")]
    public class InquiryController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly AuthManager _authManager;
        private readonly InquiryUseCase _inquiryUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryController(IMapper mapper, AuthManager authManager, InquiryUseCase inquiryUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(mapper));
            _inquiryUseCase = inquiryUseCase ?? throw new ArgumentNullException(nameof(inquiryUseCase));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Act.
            var inquiry = await _inquiryUseCase.GetAsync(id);

            // Map.
            var result = _mapper.Map<InquiryDto>(inquiry);
            result.AuditStatus = await _inquiryUseCase.GetStateAsync(result.Id);
            result.Reviewer = await _inquiryUseCase.GetReviewerAsync(result.Id);
            result.Contractor = await _inquiryUseCase.GetContractorAsync(result.Id);
            result.AccessPolicy = await _inquiryUseCase.GetAccessPolicyAsync(result.Id);
            result.CreateDate = await _inquiryUseCase.GetRecordCreateDateAsync(result.Id);

            // Return.
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            // Act.
            IAsyncEnumerable<Inquiry> inquiryList = _inquiryUseCase.GetAllAsync(pagination.Navigation);

            // Map.
            var resultList = new List<InquiryDto>();
            await foreach (var inquiry in inquiryList)
            {
                var result = _mapper.Map<InquiryDto>(inquiry);
                result.AuditStatus = await _inquiryUseCase.GetStateAsync(result.Id);
                result.Reviewer = await _inquiryUseCase.GetReviewerAsync(result.Id);
                result.Contractor = await _inquiryUseCase.GetContractorAsync(result.Id);
                result.AccessPolicy = await _inquiryUseCase.GetAccessPolicyAsync(result.Id);
                result.CreateDate = await _inquiryUseCase.GetRecordCreateDateAsync(result.Id);
                resultList.Add(result);
            }

            // Return.
            return Ok(resultList);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAsync([FromQuery] PaginationModel pagination)
        {
            // FUTURE: _inquiryUseCase.GetAllRecentAsync

            // Return.
            return Ok(await GetAllAsync(pagination));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] InquiryDto input)
        {
            // Map.
            var inquiry = _mapper.Map<Inquiry>(input);

            User sessionUser = await _authManager.GetUserAsync(User);
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);

            var attribution = new AttributionControl
            {
                Reviewer = input.Reviewer,
                Creator = sessionUser.Id,
                Owner = sessionOrganization.Id,
                Contractor = input.Contractor,
            };

            // Act.
            inquiry = await _inquiryUseCase.CreateAsync(attribution, inquiry);

            // Map.
            var result = _mapper.Map<InquiryDto>(inquiry);
            result.AuditStatus = await _inquiryUseCase.GetStateAsync(result.Id);
            result.Reviewer = await _inquiryUseCase.GetReviewerAsync(result.Id);
            result.Contractor = await _inquiryUseCase.GetContractorAsync(result.Id);
            result.AccessPolicy = await _inquiryUseCase.GetAccessPolicyAsync(result.Id);
            result.CreateDate = await _inquiryUseCase.GetRecordCreateDateAsync(result.Id);
            result.UpdateDate = await _inquiryUseCase.GetRecordUpdateDateAsync(result.Id);

            // Return.
            return Ok(result);
        }

        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        /// <param name="input">See <see cref="IFormFile"/>.</param>
        /// <returns>See <see cref="DocumentDto"/>.</returns>
        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync([Required] IFormFile input)
        {
            // FUTURE: Replace with validator?
            var virtualFile = new ApplicationFileWrapper(input, Constants.AllowedFileMimes);
            if (!virtualFile.IsValid)
            {
                throw new ArgumentException(); // TODO
            }

            // Act.
            var fileName = await _inquiryUseCase.StoreDocumentAsync(
                input.OpenReadStream(),
                input.FileName,
                input.ContentType);

            var output = new DocumentDto
            {
                Name = fileName,
            };

            // Return.
            return Ok(output);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] InquiryDto input)
        {
            // Map.
            var inquiry = _mapper.Map<Inquiry>(input);
            inquiry.Id = id;

            // Act.
            await _inquiryUseCase.UpdateAsync(inquiry);

            // Return.
            return NoContent();
        }

        [HttpPost("{id:int}/status_review")]
        public async Task<IActionResult> SetStatusReviewAsync(int id, StatusChangeDto input)
        {
            // Act.
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.PendingReview, input.Message);

            // Return.
            return NoContent();
        }

        [HttpPost("{id:int}/status_rejected")]
        public async Task<IActionResult> SetStatusRejectedAsync(int id, StatusChangeDto input)
        {
            // Act.
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.Rejected, input.Message);

            // Return.
            return NoContent();
        }

        [HttpPost("{id:int}/status_approved")]
        public async Task<IActionResult> SetStatusApprovedAsync(int id, StatusChangeDto input)
        {
            // Act.
            await _inquiryUseCase.UpdateStatusAsync(id, AuditStatus.Done, input.Message);

            // Return.
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            await _inquiryUseCase.DeleteAsync(id);

            // Return.
            return NoContent();
        }

        [HttpGet("{id:int}/download_uri")]
        public async Task<IActionResult> GetDownloadUriAsync(int id)
        {
            // Act.
            var uri = await _inquiryUseCase.GetDocumentAccessUriAsync(id);

            // Map.
            var result = new InquiryDownloadDto
            {
                Id = id,
                DownloadUri = uri
            };

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
