using AutoMapper;
using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.Portal.Controllers
{
    // TODO: Rename and refactor controller
    /// <summary>
    ///     Endpoint controller for incident operations.
    /// </summary>
    [AllowAnonymous]
    [Route("incident-portal")]
    public class IncidentPortalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentPortalController(
            IMapper mapper,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IBlobStorageService blobStorageService,
            INotifyService notifyService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        // FUTURE: Return the result in an encrypted envelope.
        // POST: api/incident-portal/upload-document
        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        /// <remarks>
        ///     Max file upload size is configured at 128 MB.
        /// </remarks>
        [HttpPost("upload-document")]
        [RequestSizeLimit(128 * 1024 * 1024)]
        public async Task<IActionResult> UploadDocumentAsync([Required][FormFile(Core.Constants.AllowedFileMimes)] IFormFile input)
        {
            // Act.
            var storeFileName = FileHelper.GetUniqueName(input.FileName);
            await _blobStorageService.StoreFileAsync(
                containerName: Core.Constants.IncidentStorageFolderName,
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

        // POST: api/incident-portal/submit
        /// <summary>
        ///     Register new incident.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> CreateIncidentAsync([FromBody] IncidentDto input, [FromServices] IIncidentService incidentService)
        {
            // Map.
            var incident = _mapper.Map<Incident>(input);

            // Act.
            await incidentService.AddAsync(incident);

            // Return.
            return NoContent();
        }

        // GET: api/incident-portal/risk
        /// <summary>
        ///     Get the analysis product by buillding identifier.
        /// </summary>
        [HttpGet("risk")]
        public async Task<IActionResult> GetRiskAnalysisAsync([Required] string id, [FromServices] IProductService productService)
        {
            // Assign.
            IAsyncEnumerable<AnalysisProduct> productList = productService.GetAnalysisAsync(AnalysisProductType.RiskPlus, id);

            // Map.
            var result = await _mapper.MapAsync<IList<AnalysisRiskPlusDto>, AnalysisProduct>(productList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
