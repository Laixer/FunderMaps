using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public OrganizationController(
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            ILookupNormalizer keyNormalizer,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        // FUTURE: Only organization users or admins
        // GET: api/organization/{id}
        [HttpGet("{id:guid}")]
        public async Task<Organization> GetAsync(Guid id)
        {
            return await _context.Organizations
                .AsNoTracking()
                .Include(a => a.HomeAddress)
                .Include(a => a.PostalAddres)
                .Where(q => q.Id == id)
                .SingleOrDefaultAsync();
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

        // POST: api/organization/proposal/{token}
        /// <summary>
        /// Create a new organization from an organization proposal. At the same time a superuser
        /// for the organization is created.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        /// <param name="input">Organization and superuser information.</param>
        [AllowAnonymous]
        [HttpPost("proposal/{token:guid}")]
        public async Task<IActionResult> FromProposalAsync([FromRoute] Guid token, [FromBody] OrganizationInitiationInputModel input)
        {
            var proposal = await _context.OrganizationProposals
                .Where(s => s.Token == token)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (proposal == null)
            {
                return NotFound();
            }

            // Create everything at once, or nothing at all if an error occurs.
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var role = await _context.OrganizationRoles
                    .Where(s => s.Name == Constants.SuperuserRole)
                    .SingleAsync();

                // Prepare user account
                var user = new FunderMapsUser(input.User.Email);
                var result = await _userManager.CreateAsync(user, input.User.Password);
                if (!result.Succeeded)
                {
                    transaction.Rollback();
                    return Conflict();
                }

                // Create organization address
                var address = new Address
                {
                    Street = input.Organization.Address,
                    AddressNumber = input.Organization.AddressNumber,
                };
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();

                // Create organization
                var organization = new Organization
                {
                    Name = proposal.Name,
                    NormalizedName = proposal.NormalizedName,
                    Email = proposal.Email,
                    HomeAddress = address,
                    PostalAddres = address,
                };
                await _context.Organizations.AddAsync(organization);
                await _context.SaveChangesAsync();

                // Attach user to organization
                await _context.OrganizationUsers.AddAsync(new OrganizationUser
                {
                    User = user,
                    Organization = organization,
                    OrganizationRole = role,
                });
                await _context.SaveChangesAsync();

                // Remove proposal
                _context.OrganizationProposals.Remove(proposal);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return Ok(organization);
            }
        }

        // POST: api/organization/{id}/new_superuser
        [HttpPost("{id:guid}/new_superuser")]
        public async Task<IActionResult> NewSuperUserAsync(Guid id, [FromBody] FunderMapsUser user)
        {
            var organization = await _context.Organizations.FindAsync(id);

            // Prepare user account
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return Conflict();
            }

            // Attach user to organization
            _context.OrganizationUsers.Add(new OrganizationUser
            {
                User = user,
                Organization = organization,
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/organization
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Organization organization)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "");
            if (authorizationResult.Succeeded)
            {
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();

                return Ok(organization);
            }

            return new ForbidResult();
        }
    }
}
