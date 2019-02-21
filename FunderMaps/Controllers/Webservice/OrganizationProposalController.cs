using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers.Webservice
{
    [Authorize(Roles = Constants.AdministratorRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationProposalController : AbstractMicroController
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

        // GET: api/organizationproposal/{token}
        /// <summary>
        /// Get the proposal by token.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpGet("{token:guid}")]
        public async Task<IActionResult> GetAsync(Guid token)
        {
            var proposal = await _context.OrganizationProposals
                .AsNoTracking()
                .Where(s => s.Token == token)
                .SingleOrDefaultAsync();

            if (proposal == null)
            {
                return NotFound();
            }

            return Ok(proposal);
        }

        // POST: api/organizationproposal
        [HttpPost]
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

            // TODO: Send email with registration link

            return Ok(proposal);
        }

        // DELETE: api/organizationproposal/{token}
        /// <summary>
        /// Remove the proposal from the datastore. This operation will
        /// not return an error if the proposal cannot be found.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpDelete("{token:guid}")]
        public async Task DeleteAsync(Guid token)
        {
            var proposal = await _context.OrganizationProposals
                .AsNoTracking()
                .Where(s => s.Token == token)
                .SingleOrDefaultAsync();

            if (proposal != null)
            {
                _context.OrganizationProposals.Remove(proposal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
