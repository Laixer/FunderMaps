using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.Webservice.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IProductService _productService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentPortalController(
            IMapper mapper,
            IncidentUseCase incidentUseCase,
            GeocoderUseCase geocoderUseCase,
            IProductService productService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _incidentUseCase = incidentUseCase ?? throw new ArgumentNullException(nameof(incidentUseCase));
            _geocoderUseCase = geocoderUseCase ?? throw new ArgumentNullException(nameof(geocoderUseCase));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        ///     Upload document to the backstore.
        /// </summary>
        /// <remarks>
        ///     Max file upload size is configured at 128 MB.
        /// </remarks>
        /// <param name="input">See <see cref="IFormFile"/>.</param>
        /// <returns>See <see cref="DocumentDto"/>.</returns>
        [HttpPost("upload-document")]
        [RequestSizeLimit(128 * 1024 * 1024)]
        public async Task<IActionResult> UploadDocumentAsync([Required] IFormFile input)
        {
            // FUTURE: Replace with validator?

            if (input.Length == 0)
            {
                throw new UploadException("File content is empty");
            }

            // Check if content type is allowed
            var allowedFileMimes = new List<string>(Constants.AllowedFileMimes);
            if (!allowedFileMimes.Contains(input.ContentType))
            {
                throw new UploadException("File content type is not allowed");
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
            IAsyncEnumerable<Address> addressList = _geocoderUseCase.GetAllBySuggestionAsync(input.Query, input.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<AddressBuildingDto>, Address>(addressList, HttpContext.RequestAborted);

            // Return.
            return Ok(result);
        }

        /// <summary>
        ///     Get the analysis product by buillding identifier.
        /// </summary>
        /// <param name="id">Address identifier.</param>
        /// <returns>The risk product.</returns>
        [HttpGet("risk")]
        public async Task<IActionResult> GetRiskAnalysisAsync([Required][Geocoder] string id)
        {
            // Assign.
            AnalysisProduct product = await _productService.GetAnalysisByIdAsync(Guid.Empty, AnalysisProductType.Risk, id, HttpContext.RequestAborted);

            // Map.
            var result = _mapper.Map<AnalysisProduct, AnalysisRiskDto>(product);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
