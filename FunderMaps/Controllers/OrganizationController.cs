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
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationController(
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organization/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var organization = await _context.Organizations
                    .AsNoTracking()
                    .Include(a => a.HomeAddress)
                    .Include(a => a.PostalAddres)
                    .Where(q => q.Id == id)
                    .SingleOrDefaultAsync();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok(organization);
            }

            return Forbid();
        }

        // FUTURE: Map user results
        // GET: api/organization/{id}/users
        [HttpGet("{id:guid}/users")]
        public async Task<IActionResult> GetUsersAsync(Guid id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            var organizationUser = await _context.OrganizationUsers
                    .AsNoTracking()
                    .Include(a => a.User)
                    .Where(q => q.Organization == organization)
                    .Select(s => s.User)
                    .ToListAsync();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok(organizationUser);
            }

            return Forbid();
        }

        public sealed class UserInputModel
        {
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string Role { get; set; }
        }

        // POST: api/organization/{id}/add_user
        [HttpPost("{id:guid}/add_user")]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] UserInputModel input)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                // Create everything at once, or nothing at all if an error occurs.
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var role = await _context.OrganizationRoles
                            .SingleAsync(s => s.NormalizedName == _keyNormalizer.Normalize(Constants.ReaderRole));

                    // Check if role exists
                    if (!string.IsNullOrEmpty(input.Role))
                    {
                        role = await _context.OrganizationRoles
                            .SingleOrDefaultAsync(s => s.NormalizedName == _keyNormalizer.Normalize(input.Role));

                        if (role == null)
                        {
                            return BadRequest();
                        }
                    }

                    // Prepare user account
                    var user = new FunderMapsUser(input.Email);
                    var result = await _userManager.CreateAsync(user, input.Password);
                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        return Conflict();
                    }

                    // Attach user to organization
                    await _context.OrganizationUsers.AddAsync(new OrganizationUser(user, organization, role));
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Ok();
                }
            }

            return Forbid();
        }

        // POST: api/organization/{id}/remove_user
        [HttpPost("{id:guid}/remove_user")]
        public async Task<IActionResult> RemoveUserAsync(Guid id, [FromBody] UserInputModel input)
        {
            var organization = await _context.Organizations.FindAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok();
            }

            return Forbid();
        }

        // PUT: api/organization/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] Organization organization)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();

                return Ok(organization);
            }

            return Forbid();
        }
    }
}
