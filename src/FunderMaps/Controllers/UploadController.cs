using FunderMaps.Core.Interfaces;
using FunderMaps.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers
{
    /// <summary>
    /// This controller deals with file uploads.
    /// </summary>
    public abstract class UploadController : BaseApiController
    {
        private readonly string _store;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="fileStorageService">The file storage service.</param>
        /// <param name="store">File storage location.</param>
        public UploadController(IFileStorageService fileStorageService, string store)
        {
            _fileStorageService = fileStorageService;
            _store = store;
        }

        /// <summary>
        /// Upload a file to the file store and return the result.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="allowedFileTypes">Allowed file types.</param>
        /// <returns>An instance of <see cref="IActionResult"/>.</returns>
        protected virtual async Task<IActionResult> Upload(IFormFile file, IEnumerable<string> allowedFileTypes)
        {
            // File *must* exist
            if (file == null)
            {
                return BadRequest(0, "file object is empty");
            }

            var fileWrapper = new ApplicationFileWrapper(file, allowedFileTypes);
            if (!fileWrapper.IsValid)
            {
                return BadRequest(0, fileWrapper.Error);
            }

            try
            {
                // Store the report with the file service
                await _fileStorageService.StoreFileAsync(_store, fileWrapper.File, file.OpenReadStream());

                // Return the file meta data
                return Ok(fileWrapper.File);
            }
            catch { return ApplicationError(); }
        }
    }
}
