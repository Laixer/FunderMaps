using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

namespace FunderMaps.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationController(FunderMapsDbContext context, UserManager<FunderMapsUser> userManager, ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _userManager = userManager;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organization/{id}
        [HttpGet("{id:guid}")]
        public async Task<Organization> Get(Guid id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public class OrganizationInitiationInputModel
        {
            [Required]
            public FunderMapsUser User { get; set; }

            [Required]
            public Organization Organization { get; set; }
        }

        // POST: api/organization/proposal/{token}
        [HttpPost("proposal/{token:guid}")]
        public async Task<IActionResult> FromProposal([FromRoute] Guid token, [FromBody] OrganizationInitiationInputModel value)
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
                organization.NormalizedName = _keyNormalizer.Normalize(proposal.Name);
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
    }
}
