using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Helpers;
using FunderMaps.ViewModels;

namespace FunderMaps.Controllers.Api
{
    [Authorize(Roles = Constants.AdministratorRole)]
    [Route("api/organization_proposal")]
    [ApiController]
    public class OrganizationProposalController : BaseApiController
    {
        private readonly FunderMapsDbContext _context;
        private readonly ILookupNormalizer _keyNormalizer;

        /// <summary>
        /// Create a new instance of the OrganizationProposalController.
        /// </summary>
        public OrganizationProposalController(
            FunderMapsDbContext context,
            ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organization_proposal/{token}
        /// <summary>
        /// Get the organization proposal by token.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpGet("{token:guid}")]
        [ProducesResponseType(typeof(OrganizationProposal), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> GetAsync(Guid token)
        {
            var proposal = await _context.OrganizationProposals.FindAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            return Ok(proposal);
        }

        // POST: api/organization_proposal
        /// <summary>
        /// Submit new organization proposal.
        /// </summary>
        /// <param name="proposal">See <see cref="OrganizationProposal"/>.</param>
        /// <returns>See <see cref="OrganizationProposal"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrganizationProposal), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 409)]
        public async Task<IActionResult> PostAsync([FromBody] OrganizationProposal proposal)
        {
            proposal.NormalizedName = _keyNormalizer.Normalize(proposal.Name);

            // Organization proposals must be unique.
            if (await _context.OrganizationProposals.AnyAsync(s => s.NormalizedName == proposal.NormalizedName))
            {
                return Conflict(proposal.Name);
            }

            // Organization proposals cannot use an existing organization name.
            if (await _context.Organizations.AnyAsync(s => s.NormalizedName == proposal.NormalizedName))
            {
                return Conflict(proposal.Name);
            }

            await _context.OrganizationProposals.AddAsync(proposal);
            await _context.SaveChangesAsync();

            // FUTURE: Send email with registration link

            return Ok(proposal);
        }

        // DELETE: api/organization_proposal/{token}
        /// <summary>
        /// Remove the proposal from the datastore. This operation will
        /// not return an error if the proposal cannot be found.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpDelete("{token:guid}")]
        [ProducesResponseType(typeof(OrganizationProposal), 204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> DeleteAsync(Guid token)
        {
            var proposal = await _context.OrganizationProposals.FindAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            _context.OrganizationProposals.Remove(proposal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
