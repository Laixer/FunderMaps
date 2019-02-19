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

        public sealed class UserInputModel
        {
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        // FUTURE: Only administrators or superusers
        // POST: api/organization/{id}/new_superuser
        [HttpPost("{id:guid}/new_superuser")]
        public async Task<IActionResult> NewSuperUserAsync(Guid id, [FromBody] UserInputModel input)
        {
            var organization = await _context.Organizations.FindAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationRolePolicy");
            if (authorizationResult.Succeeded)
            {
                // Create everything at once, or nothing at all if an error occurs.
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var role = await _context.OrganizationRoles
                        .Where(s => s.Name == Constants.SuperuserRole)
                        .SingleAsync();

                    // Prepare user account
                    var user = new FunderMapsUser(input.Email);
                    var result = await _userManager.CreateAsync(user, input.Password);
                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        return Conflict();
                    }

                    // Attach user to organization
                    _context.OrganizationUsers.Add(new OrganizationUser
                    {
                        User = user,
                        Organization = organization,
                        OrganizationRole = role,
                    });
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Ok();
                }
            }

            return Forbid();
        }

        // PUT: api/organization
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Organization organization)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
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
