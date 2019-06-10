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
using FunderMaps.ViewModels;
using FunderMaps.Core.Interfaces;
using FunderMaps.Extensions;
using FunderMaps.Data.Authorization;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Organization operation endpoint.
    /// </summary>
    [Authorize]
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : BaseApiController
    {
        private readonly FunderMapsDbContext _context;
        private readonly FisDbContext _fisContext;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILookupNormalizer _keyNormalizer;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public OrganizationController(
            FunderMapsDbContext context,
            FisDbContext fisContext,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _fisContext = fisContext;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organization
        /// <summary>
        /// Get all organizations of which the current authenticated user is a member of
        /// or get all organizations as admin.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Organization>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _context.Organizations.AsNoTracking().ToListAsync());
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            return Ok(await _context.OrganizationUsers
                .AsNoTracking()
                .Include(s => s.Organization)
                .Include(s => s.OrganizationRole)
                .Where(s => s.UserId == user.Id)
                .Select(s => s.Organization)
                .ToListAsync());
        }

        // GET: api/organization/{id}
        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var organization = await _context.Organizations
                .AsNoTracking()
                .SingleOrDefaultAsync(q => q.Id == id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok(organization);
            }

            return ResourceForbid();
        }

        public sealed class OrganizationUserInputOutputModel
        {
            [Required]
            public FunderMapsUser User { get; set; }

            public OrganizationRole Role { get; set; }
        }

        // GET: api/organization/{id}/user
        /// <summary>
        /// Get organization user list.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>List of users, see <see cref="FunderMapsUser"/>.</returns>
        [HttpGet("{id:guid}/user")]
        [ProducesResponseType(typeof(List<FunderMapsUser>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetUsersAsync(Guid id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationMemberPolicy");
            if (authorizationResult.Succeeded)
            {
                return Ok(await _context.OrganizationUsers
                    .AsNoTracking()
                    .Include(a => a.User)
                    .Where(q => q.Organization == organization)
                    .Select(s => new OrganizationUserInputOutputModel { User = s.User, Role = s.OrganizationRole })
                    .ToListAsync());
            }

            return ResourceForbid();
        }

        // POST: api/organization/{id}/user
        /// <summary>
        /// Add user to organization.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="input">See <see cref="UserInputModel"/>.</param>
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

                        var attestationPrincipal = new Core.Entities.Fis.Principal
                        {
                            NickName = input.Email,
                            Email = input.Email,
                            _Organization = organization.AttestationOrganizationId,
                        };

                        // NOTE: This can fail because entity exists
                        await _fisContext.Principal.AddAsync(attestationPrincipal);
                        await _fisContext.SaveChangesAsync();

                        // Prepare user account
                        var user = new FunderMapsUser(input.Email)
                        {
                            AttestationPrincipalId = attestationPrincipal.Id
                        };
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

        // GET: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Get organization user if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="user_id">User id.</param>
        [HttpGet("{id:guid}/user/{user_id:guid}")]
        [ProducesResponseType(typeof(OrganizationUserInputOutputModel), 204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid user_id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(user_id.ToString());
                if (user == null)
                {
                    return ResourceNotFound();
                }

                // If the user exists, but not in this organization, forbid deletion
                var organizationUser = await _context.OrganizationUsers
                    .Include(s => s.OrganizationRole)
                    .SingleAsync(s => s.User == user && s.Organization == organization);
                if (organizationUser == null)
                {
                    return ResourceForbid();
                }

                return Ok(new OrganizationUserInputOutputModel
                {
                    User = user,
                    Role = organizationUser.OrganizationRole
                });
            }

            return NoContent();
        }

        // PUT: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Update organization user if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="user_id">User id.</param>
        /// <param name="input">User object.</param>
        [HttpPut("{id:guid}/user/{user_id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid user_id, OrganizationUserInputOutputModel input)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(user_id.ToString());
                if (user == null)
                {
                    return ResourceNotFound();
                }

                // If the user exists, but not in this organization, forbid alteration
                var organizationUser = await _context.OrganizationUsers
                    .SingleAsync(s => s.User == user && s.Organization == organization);
                if (organizationUser == null)
                {
                    return ResourceForbid();
                }

                if (input.User != null)
                {
                    user.PhoneNumber = input.User.PhoneNumber;
                    user.GivenName = input.User.GivenName;
                    user.LastName = input.User.LastName;
                    user.Avatar = input.User.Avatar;
                    user.JobTitle = input.User.JobTitle;
                }

                if (input.Role != null)
                {
                    // TODO: wrap in transaction
                    var role = await _context.OrganizationRoles
                                .SingleOrDefaultAsync(s => s.NormalizedName == _keyNormalizer.Normalize(input.Role.Name));

                    if (role == null)
                    {
                        return BadRequest(0, "Role not found");
                    }

                    organizationUser.OrganizationRoleId = role.Id;

                    _context.OrganizationUsers.Update(organizationUser);
                    await _context.SaveChangesAsync();
                }

                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        // DELETE: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Remove an user from the organization if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="user_id">User id.</param>
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
                .FirstOrDefaultAsync(s => s.Id == id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organization, "OrganizationSuperuserPolicy");
            if (authorizationResult.Succeeded)
            {
                organization.Email = input.Email;
                organization.PhoneNumber = input.PhoneNumber;
                organization.RegistrationNumber = input.RegistrationNumber;
                organization.BrandingLogo = input.BrandingLogo;
                organization.InvoiceName = input.InvoiceName;
                organization.InvoicePONumber = input.InvoicePONumber;
                organization.InvoiceEmail = input.InvoiceEmail;

                organization.HomeStreet = input.HomeStreet;
                organization.HomeAddressNumber = input.HomeAddressNumber;
                organization.HomeAddressNumberPostfix = input.HomeAddressNumberPostfix;
                organization.HomeCity = input.HomeCity;
                organization.HomePostbox = input.HomePostbox;
                organization.HomeZipcode = input.HomeZipcode;
                organization.HomeState = input.HomeState;
                organization.HomeCountry = input.HomeCountry;

                organization.PostalStreet = input.PostalStreet;
                organization.PostalAddressNumber = input.PostalAddressNumber;
                organization.PostalAddressNumberPostfix = input.PostalAddressNumberPostfix;
                organization.PostalCity = input.PostalCity;
                organization.PostalPostbox = input.PostalPostbox;
                organization.PostalZipcode = input.PostalZipcode;
                organization.PostalState = input.PostalState;
                organization.PostalCountry = input.PostalCountry;

                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
