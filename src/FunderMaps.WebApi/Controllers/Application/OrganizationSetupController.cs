using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class OrganizationSetupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupController(IMapper mapper, IOrganizationRepository organizationRepository, IPasswordHasher passwordHasher)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        // FUTURE: This is anon, maybe return nothing?
        // POST: api/organization/{id}/setup
        /// <summary>
        ///     Create organization from organization proposal.
        /// </summary>
        [HttpPost("organization/{id:guid}/setup")]
        public async Task<IActionResult> CreateAsync(Guid id, [FromBody] OrganizationSetupDto input)
        {
            // Map.
            var user = new User { Email = input.Email };

            // Act.
            var passwordHash = _passwordHasher.HashPassword(input.Password);
            await _organizationRepository.AddFromProposalAsync(id, user.Email, passwordHash);
            Organization organization = await _organizationRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
