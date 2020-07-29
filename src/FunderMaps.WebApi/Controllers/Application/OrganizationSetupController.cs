using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization setup.
    /// </summary>
    /// <remarks>
    ///     Organization setup converts an already existing organization
    ///     proposal into a full organization.
    /// </remarks>
    [ApiController]
    public class OrganizationSetupController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        // TODO: This is anon, maybe return nothing?
        [HttpPost("api/organization/{id:guid}/setup")]
        public async Task<IActionResult> GetAsync(Guid id, [FromBody] OrganizationSetupDto input)
        {
            // Map.
            var user = new User { Email = input.Email };

            // Act.
            Organization organization = await _organizationManager.CreateFromProposalAsync(id, user, input.Password).ConfigureAwait(false);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
