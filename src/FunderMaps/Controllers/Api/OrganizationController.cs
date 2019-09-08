using FunderMaps.Core.Entities;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly UserManager<FunderMapsUser> _userManager;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public OrganizationController(
            IOrganizationRepository organizationRepository,
            IOrganizationUserRepository organizationUserRepository,
            UserManager<FunderMapsUser> userManager)
        {
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _userManager = userManager;
        }

        // GET: api/organization
        /// <summary>
        /// Get all organizations of which the current authenticated user is a member of.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(List<Organization>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync()
        {
            var organization = await _organizationRepository.GetByIdAsync(User.GetOrganizationId());
            if (organization == null)
            {
                return ResourceNotFound();
            }

            return Ok(organization);
        }

        // GET: api/organization/{id}
        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:guid}")]
        [Authorize(Policy = Constants.OrganizationMemberOrAdministratorPolicy)]
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            return Ok(organization);
        }

        // GET: api/organization/{id}/user
        /// <summary>
        /// Get organization user list.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of users, see <see cref="FunderMapsUser"/>.</returns>
        [HttpGet("{id:guid}/user")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(typeof(List<FunderMapsUser>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetUsersAsync(Guid id, [FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            return Ok(await _organizationUserRepository.ListAllByOrganizationIdAsync(organization.Id, new Navigation(offset, limit)));
        }

        // POST: api/organization/{id}/user
        /// <summary>
        /// Add user to organization.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="input">See <see cref="UserInputModel"/>.</param>
        [HttpPost("{id:guid}/user")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        [ProducesResponseType(typeof(ErrorOutputModel), 409)]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] UserInputModel input)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

            // Prepare new user account.
            var user = new FunderMapsUser(input.Email);

            // Set password on account.
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                return ApplicationError();
            }

            // Attach user to organization.
            await _organizationUserRepository.AddAsync(new OrganizationUser
            {
                UserId = user.Id,
                OrganizationId = organization.Id,
                Role = input.Role
            });

            return NoContent();
        }

        // GET: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Get organization user if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="userId">User id.</param>
        [HttpGet("{id:guid}/user/{userId:guid}")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(typeof(OrganizationUser), 204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetUserAsync(Guid id, Guid userId)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organizationUser = await _organizationUserRepository.GetByIdAsync(new KeyValuePair<Guid, Guid>(id, userId));
            if (organizationUser == null)
            {
                return ResourceForbid();
            }

            return Ok(organizationUser);
        }

        // PUT: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Update organization user if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="userId">User id.</param>
        /// <param name="input">User object.</param>
        [HttpPut("{id:guid}/user/{userId:guid}")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid userId, OrganizationUser input)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organizationUser = await _organizationUserRepository.GetByIdAsync(new KeyValuePair<Guid, Guid>(id, userId));
            if (organizationUser == null)
            {
                return ResourceForbid();
            }

            organizationUser.Role = input.Role;

            await _organizationUserRepository.UpdateAsync(organizationUser);

            return NoContent();
        }

        // DELETE: api/organization/{id}/user/{user_id}
        /// <summary>
        /// Remove an user from the organization if this user has access to the record.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="userId">User id.</param>
        [HttpDelete("{id:guid}/user/{userId:guid}")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> RemoveUserAsync(Guid id, Guid userId)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organizationUser = await _organizationUserRepository.GetByIdAsync(new KeyValuePair<Guid, Guid>(id, userId));
            if (organizationUser == null)
            {
                return ResourceForbid();
            }

            await _organizationUserRepository.DeleteAsync(organizationUser);

            return NoContent();
        }

        // PUT: api/organization/{id}
        /// <summary>
        /// Update organization if the user has access to the record.
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = Constants.OrganizationMemberSuperOrAdministratorPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] Organization input)
        {
            if (User.HasOrganization() && id != User.GetOrganizationId())
            {
                return ResourceForbid();
            }

            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return ResourceNotFound();
            }

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

            await _organizationRepository.UpdateAsync(organization);

            return NoContent();
        }
    }
}
