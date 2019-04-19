using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Helpers;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

namespace FunderMaps.Controllers.Webservice
{
    [AllowAnonymous]
    [Route("api/organization_registration")]
    [ApiController]
    public class OrganizationRegistrationController : BaseApiController
    {
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;

        public OrganizationRegistrationController(
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public sealed class OrganizationInitiationInputModel
        {
            public sealed class UserModel
            {
                [Required]
                [EmailAddress]
                [DataType(DataType.EmailAddress)]
                public string Email { get; set; }

                [Required]
                [DataType(DataType.Password)]
                public string Password { get; set; }
            }

            public sealed class OrganizationModel
            {
                [Required]
                public string Address { get; set; }

                [Required]
                public int AddressNumber { get; set; }
            }

            [Required]
            public UserModel User { get; set; }

            [Required]
            public OrganizationModel Organization { get; set; }
        }

        /// <summary>
        /// Create an organization with superuser from the proposal.
        /// </summary>
        /// <param name="proposal">Organization proposal.</param>
        /// <param name="input">Client input.</param>
        private async Task CreateOrganizationFromProposal(OrganizationProposal proposal, OrganizationInitiationInputModel input)
        {
            var role = await _context.OrganizationRoles.FirstAsync(s => s.Name == Constants.SuperuserRole);

            // Prepare new user account
            var user = new FunderMapsUser(input.User.Email);
            var result = await _userManager.CreateAsync(user, input.User.Password);
            if (!result.Succeeded)
            {
                // TODO: Wrap errors in exception
                throw new Exception();
            }

            // Attach attestation object to user
            //user.AttestationPrincipalId = attestationPrincipal.Id;

            // Create new organization address
            var address = new Address
            {
                Street = input.Organization.Address,
                AddressNumber = input.Organization.AddressNumber,
            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            // Create new organization
            var organization = new Organization
            {
                Name = proposal.Name,
                NormalizedName = proposal.NormalizedName,
                Email = proposal.Email,
                HomeAddress = address,
                PostalAddres = address,
                //AttestationOrganizationId = attestationOrganization.Id,
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
        public async Task<IActionResult> FromProposalAsync([FromRoute] Guid token, [FromBody] OrganizationInitiationInputModel input)
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