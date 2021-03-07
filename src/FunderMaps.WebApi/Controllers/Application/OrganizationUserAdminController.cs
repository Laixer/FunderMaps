using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization user administration.
    /// </summary>
    /// <remarks>
    ///     This controller provides organization administration.
    ///     <para>
    ///         For the variant based on the current session see 
    ///         <see cref="FunderMaps.AspNetCore.Controllers.OrganizationUserController"/>.
    ///     </para>
    /// </remarks>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("admin/organization/{id:guid}/user")]
    public class OrganizationUserAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly SignInService _signinService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserAdminController(
            IMapper mapper,
            IUserRepository userRepository,
            IOrganizationUserRepository organizationUserRepository,
            SignInService signinService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            _signinService = signinService ?? throw new ArgumentNullException(nameof(signinService));
        }

        // POST: api/admin/organization/{id}/user
        /// <summary>
        ///     Add user to organization.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] OrganizationUserPasswordDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            // FUTURE: Do in 1 call.
            user = await _userRepository.AddGetAsync(user);
            await _signinService.SetPasswordAsync(user.Id, input.Password);
            await _organizationUserRepository.AddAsync(id, user.Id, input.OrganizationRole);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        // GET: api/admin/organization/{id}/user
        /// <summary>
        ///     Get all users in the organization.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(Guid id, [FromQuery] PaginationDto pagination)
        {
            // Act.
            // TODO: FIX: This is ugly.
            // TODO: Single call
            var output = new List<OrganizationUserDto>();
            await foreach (var user in _organizationUserRepository.ListAllAsync(id, pagination.Navigation))
            {
                var result = _mapper.Map<OrganizationUserDto>(await _userRepository.GetByIdAsync(user));
                result.OrganizationRole = await _organizationUserRepository.GetOrganizationRoleByUserIdAsync(user);
                output.Add(result);
            }

            // Return.
            return Ok(output);
        }

        // PUT: api/admin/organization/{id}/user/{id}
        /// <summary>
        ///     Update user in the organization.
        /// </summary>
        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid userId, [FromBody] OrganizationUserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = userId;

            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(id, user.Id))
            {
                throw new AuthorizationException();
            }
            await _userRepository.UpdateAsync(user);

            // Return.
            return NoContent();
        }

        // POST: api/organization/user/{id}/change-organization-role
        /// <summary>
        ///     Set user organization role in the session organization.
        /// </summary>
        [HttpPost("{userId:guid}/change-organization-role")]
        public async Task<IActionResult> ChangeOrganizationUserRoleAsync(Guid id, Guid userId, [FromBody] ChangeOrganizationRoleDto input)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(id, userId))
            {
                throw new AuthorizationException();
            }
            await _organizationUserRepository.SetOrganizationRoleByUserIdAsync(userId, input.Role);

            // Return.
            return NoContent();
        }

        // FUTURE: Remove old password from DTO
        // POST: api/organization/user/{id}/change-password
        /// <summary>
        ///     Set user password in the session organization.
        /// </summary>
        [HttpPost("{userId:guid}/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(Guid id, Guid userId, [FromBody] ChangePasswordDto input)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(id, userId))
            {
                throw new AuthorizationException();
            }

            // Act.
            await _signinService.SetPasswordAsync(userId, input.NewPassword);

            // Return.
            return NoContent();
        }

        // DELETE: api/admin/organization/{id}/user/{id}
        /// <summary>
        ///     Delete user in the organization.
        /// </summary>
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, Guid userId)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(id, userId))
            {
                throw new AuthorizationException();
            }
            await _userRepository.DeleteAsync(userId);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
