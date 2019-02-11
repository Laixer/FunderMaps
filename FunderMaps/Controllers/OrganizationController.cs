using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;

namespace FunderMaps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly FunderMapsDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Data.Identity.FunderMapsUser> _userManager;

        public OrganizationController(FunderMapsDbContext context, Microsoft.AspNetCore.Identity.UserManager<Data.Identity.FunderMapsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/organization
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/organization/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/organization/proposal/{token}
        [HttpPost("proposal/{token:guid}")]
        public async Task<IActionResult> FromProposal([FromRoute] Guid token, [FromBody] OrganizationInitiation value)
        {
            var proposal = await _context.OrganizationProposals
                .Where(s => s.Token == token)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (proposal == null)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var user = value.User;
                var organization = value.Organization;

                // Prepare user account
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    transaction.Rollback();
                    return Conflict();
                }

                // Create organization
                organization.Id = Guid.NewGuid();
                organization.Name = proposal.Name;
                organization.Email = proposal.Email;
                _context.Organizations.Add(organization);
                await _context.SaveChangesAsync();

                // Attach user to organization
                _context.OrganizationUsers.Add(new OrganizationUser
                {
                    User = user,
                    Organization = organization,
                });
                await _context.SaveChangesAsync();

                // Remove proposal
                _context.OrganizationProposals.Remove(proposal);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(organization);
            }
        }

        // PUT: api/organization/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/organization/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
