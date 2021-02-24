using AutoMapper;
using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
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
        private readonly IRecoveryRepository _recoveryRepository;
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryController(
            IMapper mapper,
            IRecoveryRepository recoveryRepository,
            IBlobStorageService blobStorageService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _recoveryRepository = recoveryRepository ?? throw new ArgumentNullException(nameof(recoveryRepository));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        // GET: api/recovery/stats
        /// <summary>
        ///     Return recovery statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatsAsync()
        {
            // Map.
            DatasetStatsDto output = new()
            {
                Count = await _recoveryRepository.CountAsync(),
            };

            // Return.
            return Ok(output);
        }

        // GET: api/recovery/{id}
        /// <summary>
        ///     Return recovery by id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Act.
            Recovery recovery = await _recoveryRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<RecoveryDto>(recovery);

            // Return.
            return Ok(output);
        }

        // GET: api/recovery
        /// <summary>
        ///     Return all recoveries.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<Recovery> organizationList = _recoveryRepository.ListAllAsync(pagination.Navigation);

            // Map.
            IList<RecoveryDto> output = await _mapper.MapAsync<IList<RecoveryDto>, Recovery>(organizationList);

            // Return.
            return Ok(output);
        }

        // POST: api/recovery
        /// <summary>
        ///     Create recovery.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecoveryDto input)
        {
            // Map.
            var recovery = _mapper.Map<Recovery>(input);

            // Act.
            recovery = await _recoveryRepository.AddGetAsync(recovery);

            // Map.
            var output = _mapper.Map<RecoveryDto>(recovery);

            // Return.
            return Ok(output);
        }

        // POST: api/recovery/upload-document
        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        [HttpPost("upload-document")]
        [RequestSizeLimit(128 * 1024 * 1024)]
        public async Task<IActionResult> UploadDocumentAsync([Required][FormFile(Core.Constants.AllowedFileMimes)] IFormFile input)
        {
            // Act.
            string storeFileName = FileHelper.GetUniqueName(input.FileName);
            await _blobStorageService.StoreFileAsync(
                containerName: Core.Constants.RecoveryStorageFolderName,
                fileName: storeFileName,
                contentType: input.ContentType,
                stream: input.OpenReadStream());

            DocumentDto output = new()
            {
                Name = storeFileName,
            };

            // Return.
            return Ok(output);
        }

        // GET: api/recovery/{id}/download
        /// <summary>
        ///     Retrieve document access link.
        /// </summary>
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> GetDocumentAccessLinkAsync(int id)
        {
            // Act.
            Recovery recovery = await _recoveryRepository.GetByIdAsync(id);
            Uri link = await _blobStorageService.GetAccessLinkAsync(
                containerName: Core.Constants.InquiryStorageFolderName,
                fileName: recovery.DocumentFile,
                hoursValid: 1);

            // Map.
            BlobAccessLinkDto result = new()
            {
                AccessLink = link
            };

            // Return.
            return Ok(result);
        }

        // PUT: api/recovery/{id}
        /// <summary>
        ///     Update recovery by id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RecoveryDto input)
        {
            // Map.
            var recovery = _mapper.Map<Recovery>(input);
            recovery.Id = id;

            // Act.
            await _recoveryRepository.UpdateAsync(recovery);

            // Return.
            return NoContent();
        }

        // DELETE: api/recovery/{id}
        /// <summary>
        ///     Delete recovery by id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            await _recoveryRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
