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
        private static readonly Guid testOrgId = Guid.Parse("2bd4569a-0e25-4312-b3a6-7f9f7546ffd9"); // TODO: Get from context

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

        [HttpPost("api/organization/setup")]
        public async Task<IActionResult> GetAsync()
        {
            var user = new User { GivenName = "kaas", Email = "owiie@kaaas.comz" };

            // Act.
            Organization organization = await _organizationManager.CreateFromProposalAsync(testOrgId, user, "ABC@123").ConfigureAwait(false);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
