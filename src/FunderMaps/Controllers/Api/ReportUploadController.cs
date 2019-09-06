using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Helpers;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// This endpoint deals with report file uploads.
    /// </summary>
    [Authorize]
    [Route("api/report/upload")]
    [ApiController]
    public class ReportUploadController : UploadController
    {
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

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="fileStorageService">The file storage service.</param>
        public ReportUploadController(IFileStorageService fileStorageService)
            : base(fileStorageService, Constants.ReportStorage)
        { }

        // POST: api/upload
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
            => await Upload(file, allowedReportFileTypes);
    }
}
