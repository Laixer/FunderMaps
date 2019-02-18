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

namespace FunderMaps.Controllers
{
    [Authorize(Roles = Constants.AdministratorRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationProposalController : ControllerBase
    {
        private readonly FunderMapsDbContext _context;
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationProposalController(FunderMapsDbContext context, ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organizationproposal/{token}
        [HttpGet("{token:guid}")]
        public async Task<IActionResult> Get(Guid token)
        {
            var proposal = await _context.OrganizationProposals
                .Where(s => s.Token == token)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (proposal == null)
            {
                return NotFound();
            }

            return Ok(proposal);
        }

        // POST: api/organizationproposal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationProposal proposal)
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
        [HttpDelete("{token:guid}")]
        public async Task Delete(Guid token)
        {
            var proposal = await _context.OrganizationProposals
                .Where(s => s.Token == token)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (proposal != null)
            {
                _context.OrganizationProposals.Remove(proposal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
