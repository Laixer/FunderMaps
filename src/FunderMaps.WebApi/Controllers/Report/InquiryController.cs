using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Helpers;
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
    ///     Endpoint controller for inquiry operations.
    /// </summary>
    [Route("inquiry")]
    public class InquiryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IBlobStorageService _fileStorageService;
        private readonly INotificationService _notificationService;

        // TODO Move to some constant file.
        /// <summary>
        ///     Inquiry storage destination folder name.
        /// </summary>
        internal const string InquiryStorageFolderName = "inquiry-report";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryController(
            IMapper mapper,
            Core.AppContext appContext,
            IInquiryRepository inquiryRepository,
            IBlobStorageService fileStorageService,
            INotificationService notificationService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        // GET: api/inquiry/stats
        /// <summary>
        ///     Return inquiry statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetCountAsync()
        {
            // Map.
            var output = new DatasetStatsDto
            {
                Count = await _inquiryRepository.CountAsync(),
            };

            // Return.
            return Ok(output);
        }

        // GET: api/inquiry/{id}
        /// <summary>
        ///     Return inquiry by id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<InquiryDto>(inquiry);

            // Return.
            return Ok(output);
        }

        // GET: api/inquiry
        /// <summary>
        ///     Return all inquiries.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<InquiryFull> organizationList = _inquiryRepository.ListAllAsync(pagination.Navigation);

            // Map.
            var output = await _mapper.MapAsync<IList<InquiryDto>, InquiryFull>(organizationList);

            // Return.
            return Ok(output);
        }

        // POST: api/inquiry
        /// <summary>
        ///     Create inquiry.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] InquiryDto input)
        {
            // Map.
            var inquiry = _mapper.Map<InquiryFull>(input);

            // Act.
            inquiry = await _inquiryRepository.AddGetAsync(inquiry);

            // Map.
            var output = _mapper.Map<InquiryDto>(inquiry);

            // Return.
            return Ok(output);
        }

        // POST: api/inquiry/upload-document
        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
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
            var storeFileName = Core.IO.Path.GetUniqueName(input.FileName);
            await _fileStorageService.StoreFileAsync(
                containerName: InquiryStorageFolderName,
                fileName: storeFileName,
                contentType: input.ContentType,
                stream: input.OpenReadStream());

            var output = new DocumentDto
            {
                Name = storeFileName,
            };

            // Return.
            return Ok(output);
        }

        // GET: api/inquiry/download
        /// <summary>
        ///     Retrieve document access link.
        /// </summary>
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> GetDocumentAccessLinkAsync(int id)
        {
            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            var link = await _fileStorageService.GetAccessLinkAsync(
                containerName: InquiryStorageFolderName,
                fileName: inquiry.DocumentFile,
                hoursValid: 1);

            // Map.
            var result = new BlobAccessLinkDto
            {
                AccessLink = link
            };

            // Return.
            return Ok(result);
        }

        // PUT: api/inquiry/{id}
        /// <summary>
        ///     Update inquiry by id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] InquiryDto input)
        {
            // Map.
            var inquiry = _mapper.Map<InquiryFull>(input);
            inquiry.Id = id;

            // Act.
            await _inquiryRepository.UpdateAsync(inquiry);

            // Return.
            return NoContent();
        }

        // POST: api/inquiry/{id}/status_review
        /// <summary>
        ///     Set inquiry status to review by id.
        /// </summary>
        [HttpPost("{id:int}/status_review")]
        public async Task<IActionResult> SetStatusReviewAsync(int id, StatusChangeDto input)
        {
            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(id);

            // Transition.
            inquiry.State.TransitionToReview();

            // Act.
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);
            await _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, input.Message);

            // Return.
            return NoContent();
        }

        // POST: api/inquiry/{id}/status_rejected
        /// <summary>
        ///     Set inquiry status to rejected by id.
        /// </summary>
        [HttpPost("{id:int}/status_rejected")]
        public async Task<IActionResult> SetStatusRejectedAsync(int id, StatusChangeDto input)
        {
            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(id);

            // Transition.
            inquiry.State.TransitionToRejected();

            // Act.
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);
            await _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, input.Message);

            // Return.
            return NoContent();
        }

        // POST: api/inquiry/{id}/status_approved
        /// <summary>
        ///     Set inquiry status to done by id.
        /// </summary>
        [HttpPost("{id:int}/status_approved")]
        public async Task<IActionResult> SetStatusApprovedAsync(int id, StatusChangeDto input)
        {
            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(id);

            // Transition.
            inquiry.State.TransitionToDone();

            // Act.
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

            // Return.
            return NoContent();
        }

        // DELETE: api/inquiry/{id}
        /// <summary>
        ///     Delete inquiry by id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            await _inquiryRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
