using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.DataAnnotations;
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
using System.Net;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for incident operations.
    /// </summary>
    [Route("incident")]
    public class IncidentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IBlobStorageService _blobStorageService;

        // TODO Move to some constant file.
        /// <summary>
        ///     Incident storage destination folder name.
        /// </summary>
        internal const string IncidentStorageFolderName = "incident-report";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentController(
            IMapper mapper,
            Core.AppContext appContext,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IAddressRepository addressRepository,
            IBlobStorageService blobStorageService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        // GET: api/incident/stats
        /// <summary>
        ///     Return incident statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetCountAsync()
        {
            // Map.
            var output = new DatasetStatsDto
            {
                Count = await _incidentRepository.CountAsync(),
            };

            // Return.
            return Ok(output);
        }

        // GET: api/incident/{id}
        /// <summary>
        ///     Return incident by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([Incident] string id)
        {
            // Act.
            var incident = await _incidentRepository.GetByIdAsync(id);
            incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);

            // Map.
            var output = _mapper.Map<IncidentDto>(incident);

            // Return.
            return Ok(output);
        }

        // POST: api/incident/upload-document
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
            await _blobStorageService.StoreFileAsync(
                containerName: IncidentStorageFolderName,
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
        public async Task<IActionResult> GetDocumentAccessLinkAsync([Incident] string id)
        {
            // Act.
            var incident = await _incidentRepository.GetByIdAsync(id);
            var link = await _blobStorageService.GetAccessLinkAsync(
                containerName: IncidentStorageFolderName,
                fileName: incident.DocumentFile[0], // TODO
                hoursValid: 1);

            // Map.
            var result = new BlobAccessLinkDto
            {
                AccessLink = link
            };

            // Return.
            return Ok(result);
        }

        // GET: api/incident
        /// <summary>
        ///     Return all incidents.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            var incidentList = new List<Incident>();
            await foreach (var incident in _incidentRepository.ListAllAsync(pagination.Navigation))
            {
                incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);
                incidentList.Add(incident);
            }

            // Map.
            var output = _mapper.Map<IList<IncidentDto>>(incidentList);

            // Return.
            return Ok(output);
        }

        // POST: api/incident
        /// <summary>
        ///     Create incident.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAsync([FromBody] IncidentDto input)
        {
            // Map.
            var incident = _mapper.Map<Incident>(input);

            incident.Meta = new
            {
                SessionUser = _appContext.UserId,
                SessionOrganization = _appContext.TenantId,
                Gateway = Constants.IncidentGateway,
            };

            // Act.
            // There does not have to be a contact, but if it exists we'll save it.
            if (incident.ContactNavigation != null)
            {
                await _contactRepository.AddAsync(incident.ContactNavigation);
            }

            // FUTURE: Works for now, but may not be the best solution to check
            //         if input data is valid
            await _addressRepository.GetByIdAsync(incident.Address);

            var id = await _incidentRepository.AddAsync(incident);
            incident = await _incidentRepository.GetByIdAsync(id);
            incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);

            // Map.
            var output = _mapper.Map<IncidentDto>(incident);

            // Return.
            return Ok(output);
        }

        // PUT: api/incident/{id}
        /// <summary>
        ///     Update incident by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([Incident] string id, [FromBody] IncidentDto input)
        {
            // Map.
            var incident = _mapper.Map<Incident>(input);
            incident.Id = id;

            // Act.
            await _incidentRepository.UpdateAsync(incident);

            // Return.
            return NoContent();
        }

        // DELETE: api/incident/{id}
        /// <summary>
        ///     Delete incident by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([Incident] string id)
        {
            // Act.
            await _incidentRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
