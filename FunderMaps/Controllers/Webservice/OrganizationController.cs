using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers.Webservice
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : AbstractMicroController
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
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var organization = await _context.Organizations
                    .AsNoTracking()
                    .Include(a => a.HomeAddress)
                    .Include(a => a.PostalAddres)
                    .SingleOrDefaultAsync(q => q.Id == id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok(organization);
            }

            return ResourceForbid();
        }

        // FUTURE: Map user results
        // GET: api/organization/{id}/user
        [HttpGet("{id:guid}/user")]
        [ProducesResponseType(typeof(List<FunderMapsUser>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
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

            return ResourceForbid();
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

        // POST: api/organization/{id}/user
        [HttpPost("{id:guid}/user")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        [ProducesResponseType(typeof(ErrorOutputModel), 409)]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] UserInputModel input)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                return await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
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
                                return BadRequest(0, "Role not found");
                            }
                        }

                        // Prepare user account
                        var user = new FunderMapsUser(input.Email);
                        var result = await _userManager.CreateAsync(user, input.Password);
                        if (!result.Succeeded)
                        {
                            transaction.Rollback();
                            return ResourceExists();
                        }

                        // Attach user to organization
                        await _context.OrganizationUsers.AddAsync(new OrganizationUser(user, organization, role));
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return NoContent();
                    }
                });
            }

            return ResourceForbid();
        }

        // DELETE: api/organization/{id}/user/{id}
        /// <summary>
        /// Remove an user from the organization if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="user_id">User id.</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/user/{user_id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> RemoveUserAsync(Guid id, Guid user_id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                return await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    // Create everything at once, or nothing at all if an error occurs.
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        var user = await _userManager.FindByIdAsync(user_id.ToString());
                        if (user == null)
                        {
                            return ResourceNotFound();
                        }

                        // If the user exists, but not in this organization, forbid deletion
                        var organizationUser = await _context.OrganizationUsers
                            .SingleAsync(s => s.User == user && s.Organization == organization);
                        if (organizationUser == null)
                        {
                            return ResourceForbid();
                        }

                        // Remove the user from the organization and then delete the user
                        _context.OrganizationUsers.Remove(organizationUser);
                        await _userManager.DeleteAsync(user);

                        transaction.Commit();

                        return NoContent();
                    }
                });
            }

            return ResourceForbid();
        }

        // PUT: api/organization/{id}
        /// <summary>
        /// Update organization if the user has access to the record.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] Organization input)
        {
            var organization = await _context.Organizations
                .Include(s => s.HomeAddress)
                .Include(s => s.PostalAddres)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            organization.Email = input.Email;
            organization.PhoneNumber = input.PhoneNumber;
            organization.RegistrationNumber = input.RegistrationNumber;
            organization.BrandingLogo = input.BrandingLogo;
            organization.InvoiceName = input.InvoiceName;
            organization.InvoicePONumber = input.InvoicePONumber;
            organization.InvoiceEmail = input.InvoiceEmail;

            organization.HomeAddress.Reassign(input.HomeAddress);
            organization.PostalAddres.Reassign(input.PostalAddres);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
