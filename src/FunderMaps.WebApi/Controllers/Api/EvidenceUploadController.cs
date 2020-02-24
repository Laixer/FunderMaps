using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// This endpoint deals with evidence file uploads.
    /// </summary>
    [Authorize]
    [Route("api/evidence/upload")]
    [ApiController]
    public class EvidenceUploadController : UploadController
    {
        private static readonly string[] allowedEvidenceFileTypes =
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
        public EvidenceUploadController(IFileStorageService fileStorageService)
            : base(fileStorageService, Constants.EvidenceStorage)
        {
        }

        // POST: api/upload
        /// <summary>
        /// Upload a file stream and store the contents.
        /// </summary>
        /// <param name="file">See <see cref="IFormFile"/>.</param>
        /// <returns>See <see cref="ApplicationFile"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(IFormFile file) =>
            await Upload(file, allowedEvidenceFileTypes);
    }
}
