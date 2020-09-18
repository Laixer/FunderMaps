using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Route("organization")]
    public class OrganizationController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly AuthManager _authManager;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationController(IMapper mapper, AuthManager authManager, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        // GET: api/organization
        /// <summary>
        ///     Return session organization.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);

            // Map.
            var output = _mapper.Map<OrganizationDto>(sessionOrganization);

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
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            organization.Id = sessionOrganization.Id;

            // Act.
            await _organizationManager.UpdateAsync(organization);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
