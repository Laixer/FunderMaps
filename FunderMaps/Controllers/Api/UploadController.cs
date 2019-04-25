using System;
using System.Threading.Tasks;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Helpers;
using FunderMaps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// This endpoint deals with file uploads.
    /// </summary>
    [Authorize]
    [Route("api/upload")]
    [ApiController]
    public class UploadController : BaseApiController
    {
        private readonly IFileStorageService _fileStorageService;

        private static readonly string[] allowedReportFileTypes =
        {
            "application/pdf",
            "image/png",
            "image/jpeg",
            "image/gif",
            "image/bmp",
            "image/tiff",
            "image/webp",
        };

        public UploadController(IFileStorageService fileStorageService) => _fileStorageService = fileStorageService;

        // PUT: api/upload
        /// <summary>
        /// Upload a file stream and store the contents.
        /// </summary>
        /// <param name="file">See <see cref="IFormFile"/>.</param>
        /// <returns>See <see cref="ApplicationFile"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationFile), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync(IFormFile file)
        {
            // File must exist
            if (file == null)
            {
                return BadRequest(0, "file object is empty");
            }

            var reportDocument = new ApplicationFileWrapper(file, allowedReportFileTypes);
            if (!reportDocument.IsValid)
            {
                return BadRequest(0, reportDocument.Error);
            }

            try
            {
                // Store the report with the file service
                await _fileStorageService.StoreFileAsync("report", reportDocument.File, file.OpenReadStream());

                // Return the file meta data
                return Ok(reportDocument.File);
            }
            catch { return ApplicationError(); }
        }
    }
}
