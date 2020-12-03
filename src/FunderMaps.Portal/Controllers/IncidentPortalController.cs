using AutoMapper;
using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
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
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProductService _productService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly INotifyService _notifyService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentPortalController(
            IMapper mapper,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IAddressRepository addressRepository,
            IProductService productService,
            IBlobStorageService blobStorageService,
            INotifyService notifyService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
        }

        // POST: api/incident-portal/upload-document
        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        /// <remarks>
        ///     Max file upload size is configured at 128 MB.
        /// </remarks>
        [HttpPost("upload-document")]
        [RequestSizeLimit(128 * 1024 * 1024)]
        public async Task<IActionResult> UploadDocumentAsync([Required][FormFile(Core.IO.File.AllowedFileMimes)] IFormFile input)
        {
            // Act.
            var storeFileName = Core.IO.Path.GetUniqueName(input.FileName);
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
        public async Task<IActionResult> CreateIncidentAsync([FromBody] IncidentDto input)
        {
            // Map.
            var incident = _mapper.Map<Incident>(input);
            incident.Meta = new
            {
                UserAgent = Request.Headers["User-Agent"].ToString(),
                RemoteAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Gateway = Constants.IncidentGateway,
            };

            // Act.
            // There does not have to be a contact, but if it exists we'll save it.
            if (incident.ContactNavigation is not null && !string.IsNullOrEmpty(incident.ContactNavigation.Email))
            {
                await _contactRepository.AddAsync(incident.ContactNavigation);
            }

            // Act.
            var id = await _incidentRepository.AddAsync(incident);

            // Act.
            await _notifyService.DispatchNotifyAsync("incident_notify", new()
            {
                Recipients = new List<string> { "info@fundermaps.com", "info@kcaf.nl" }, // TODO: Retrieve from config
                Items = new Dictionary<string, string> { { "id", id } },
            });

            // Return.
            return NoContent();
        }

        // TODO: Remove?
        /// <summary>
        ///     Get address suggestions.
        /// </summary>
        [HttpGet("address-suggest")]
        public async Task<IActionResult> GetAllAddressSuggestionAsync([FromQuery] AddressSearchDto input)
        {
            // Assign.
            IAsyncEnumerable<Address> addressList = _addressRepository.GetBySearchQueryAsync(input.Query, input.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<AddressBuildingDto>, Address>(addressList, HttpContext.RequestAborted);

            // Return.
            return Ok(result);
        }

        // GET: api/incident-portal/risk
        /// <summary>
        ///     Get the analysis product by buillding identifier.
        /// </summary>
        [HttpGet("risk")]
        public async Task<IActionResult> GetRiskAnalysisAsync([Required] string id)
        {
            // Assign.
            AnalysisProduct product = await _productService.GetAnalysisByAddressExternalIdAsync(Guid.Empty, AnalysisProductType.Risk, id);

            // Map.
            var result = _mapper.Map<AnalysisProduct, AnalysisRiskDto>(product);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
