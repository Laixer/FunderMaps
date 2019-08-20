using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Helpers;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;

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
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public OrganizationRegistrationController(
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Create an organization with superuser from the proposal.
        /// </summary>
        /// <param name="proposal">Organization proposal.</param>
        /// <param name="input">Client input.</param>
        private async Task CreateOrganizationFromProposal(OrganizationProposal proposal, UserInputModel input)
        {
            var role = await _context.OrganizationRoles.FirstAsync(s => s.Name == Constants.SuperuserRole);

            var attestationOrganization = new Core.Entities.Fis.Organization
            {
                Name = proposal.Name
            };

            // TODO: Add organization to attestation

            var attestationPrincipal = new Core.Entities.Fis.Principal
            {
                NickName = input.Email,
                Email = input.Email,
                Organization = attestationOrganization,
            };

            // TODO: Add principal to attestation

            // Prepare new user account
            var user = new FunderMapsUser(input.Email)
            {
                AttestationPrincipalId = attestationPrincipal.Id
            };

            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                // TODO: Wrap errors in exception
                throw new Exception();
            }

            // Create new organization
            var organization = new Organization
            {
                Name = proposal.Name,
                NormalizedName = proposal.NormalizedName,
                Email = proposal.Email,
                AttestationOrganizationId = attestationOrganization.Id,
            };
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();

            // Attach user to organization with superuser role
            await _context.OrganizationUsers.AddAsync(new OrganizationUser(user, organization, role));
            await _context.SaveChangesAsync();

            // Remove proposal
            _context.OrganizationProposals.Remove(proposal);
            await _context.SaveChangesAsync();
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
            var proposal = await _context.OrganizationProposals.FindAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            return await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                // Create everything at once, or nothing at all if an error occurs.
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await CreateOrganizationFromProposal(proposal, input);
                        transaction.Commit();

                        return NoContent();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return ResourceExists();
                    }
                }
            });
        }
    }
}