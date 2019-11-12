using FunderMaps.Core.Entities;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint for new organizations. This turns an organization proposal
    /// into an actual organization.
    /// </summary>
    [AllowAnonymous]
    [Route("api/organization_registration")]
    [ApiController]
    public class OrganizationRegistrationController : BaseApiController
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IOrganizationProposalRepository _organizationProposalRepository;
        private readonly UserManager<FunderMapsUser> _userManager;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public OrganizationRegistrationController(
            IOrganizationRepository organizationRepository,
            IOrganizationUserRepository organizationUserRepository,
            IOrganizationProposalRepository organizationProposalRepository,
            UserManager<FunderMapsUser> userManager)
        {
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _organizationProposalRepository = organizationProposalRepository;
            _userManager = userManager;
        }

        // POST: api/organization/proposal/{token}
        /// <summary>
        /// Create a new organization from an organization proposal. At the same time a superuser
        /// for the organization is created.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        /// <param name="input">Organization and superuser information.</param>
        [HttpPost("proposal/{token:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 409)]
        public async Task<IActionResult> FromProposalAsync([FromRoute] Guid token, [FromBody] UserInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // FUTURE: Wrap inside transaction. Create everything at once, or nothing at all if an error occurs.

            var proposal = await _organizationProposalRepository.GetByIdAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            var user = new FunderMapsUser(input.Email);

            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                return ApplicationError();
            }

            // FUTURE: This should be handled in a single call.

            var organization = new Organization
            {
                Name = proposal.Name,
                NormalizedName = proposal.NormalizedName,
                Email = proposal.Email,
            };

            organization.Id = await _organizationRepository.AddAsync(organization);
            await _organizationUserRepository.AddAsync(new OrganizationUser
            {
                UserId = user.Id,
                OrganizationId = organization.Id,
                Role = input.Role
            });
            await _organizationProposalRepository.DeleteAsync(proposal);

            return NoContent();
        }
    }
}