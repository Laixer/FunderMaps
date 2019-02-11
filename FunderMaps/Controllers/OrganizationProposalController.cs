using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;

namespace FunderMaps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationProposalController : ControllerBase
    {
        private readonly FunderMapsDbContext _context;

        public OrganizationProposalController(FunderMapsDbContext context)
        {
            _context = context;
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
        [ProducesResponseType(typeof(OrganizationProposal), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] OrganizationProposal proposal)
        {
            // Organization proposals must be unique.
            if (await _context.OrganizationProposals.FindAsync(proposal.Name) != null)
            {
                return Conflict(proposal.Name);
            }

            // TODO: Check if organization name already exists

            await _context.OrganizationProposals.AddAsync(proposal);
            await _context.SaveChangesAsync();

            // TODO: Send email with registration link

            return Ok(proposal);
        }
    }
}
