using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     Endpoint controller for organization operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Authorize, Route("organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationController(IMapper mapper, Core.AppContext appContext, IOrganizationRepository organizationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }

        // GET: api/organization
        /// <summary>
        ///     Return session organization.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            Organization organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }

        // PUT: api/organization
        /// <summary>
        ///     Update session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] OrganizationDto input)
        {
            // Map.
            var organization = _mapper.Map<Organization>(input);
            organization.Id = _appContext.TenantId;

            // Act.
            await _organizationRepository.UpdateAsync(organization);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
