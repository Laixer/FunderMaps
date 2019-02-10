using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // POST: api/organizationproposal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationProposal proposal)
        {
            // Organization proposals must be unique.
            if (await _context.OrganizationProposals.FindAsync(proposal.Name) != null)
            {
                return Conflict(proposal.Name);
            }

            await _context.OrganizationProposals.AddAsync(proposal);
            await _context.SaveChangesAsync();

            return Ok(proposal);
        }
    }
}
