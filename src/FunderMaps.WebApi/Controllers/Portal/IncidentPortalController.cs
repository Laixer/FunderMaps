using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.Helpers;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Portal
{
    /// <summary>
    ///     Endpoint controller for incident operations.
    /// </summary>
    [AllowAnonymous]
    [Route("incident-portal")]
    public class IncidentPortalController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IncidentUseCase _incidentUseCase;
        private readonly GeocoderUseCase _geocoderUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentPortalController(IMapper mapper, IncidentUseCase incidentUseCase, GeocoderUseCase geocoderUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _incidentUseCase = incidentUseCase ?? throw new ArgumentNullException(nameof(incidentUseCase));
            _geocoderUseCase = geocoderUseCase ?? throw new ArgumentNullException(nameof(geocoderUseCase));
        }

        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        /// <param name="input">See <see cref="IFormFile"/>.</param>
        /// <returns>See <see cref="DocumentDto"/>.</returns>
        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocumentAsync(IFormFile input)
        {
            // FUTURE: Replace with validator?
            var virtualFile = new ApplicationFileWrapper(input, Constants.AllowedFileMimes);
            if (!virtualFile.IsValid)
            {
                throw new ArgumentException(); // TODO
            }

            // Act.
            var fileName = await _incidentUseCase.StoreDocumentAsync(
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

        /// <summary>
        ///     Register new incident.
        /// </summary>
        /// <param name="input">See <see cref="IncidentDto"/>.</param>
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
            await _incidentUseCase.CreateAsync(incident);

            // Return.
            return NoContent();
        }

        /// <summary>
        ///     Get address suggestions.
        /// </summary>
        /// <param name="input">Address search query.</param>
        /// <returns>List of matching addresses.</returns>
        [HttpGet("address-suggest")]
        public async Task<IActionResult> GetAllAddressSuggestionAsync([FromQuery] AddressSearchDto input)
        {
            // Assign.
            IAsyncEnumerable<Address> addressList = _geocoderUseCase.GetAllBySuggestionWithBuildingAsync(input.Query, input.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<AddressBuildingDto>, Address>(addressList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
