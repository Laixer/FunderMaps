using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     Endpoint controller for organization user operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle organization operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Authorize, Route("organization/user")]
    public class OrganizationUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly SignInService _signInService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserController(
            IMapper mapper,
            Core.AppContext appContext,
            IUserRepository userRepository,
            IOrganizationUserRepository organizationUserRepository,
            SignInService signInService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
        }

        // POST: organization/user
        /// <summary>
        ///     Add user to session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> AddUserAsync([FromBody] OrganizationUserPasswordDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            // FUTURE: Do in 1 call.
            user = await _userRepository.AddGetAsync(user);
            await _signInService.SetPasswordAsync(user.Id, input.Password);
            await _organizationUserRepository.AddAsync(_appContext.TenantId, user.Id, input.OrganizationRole);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        // GET: organization/user
        /// <summary>
        ///     Get all users in the session organization.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<OrganizationUserDto>>> GetAllUserAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            // TODO: FIX: This is ugly.
            // TODO: Single call
            List<OrganizationUserDto> output = new();
            await foreach (var user in _organizationUserRepository.ListAllAsync(_appContext.TenantId, pagination.Navigation))
            {
                var result = _mapper.Map<OrganizationUserDto>(await _userRepository.GetByIdAsync(user));
                result.OrganizationRole = await _organizationUserRepository.GetOrganizationRoleByUserIdAsync(user);
                output.Add(result);
            }

            // Return.
            return Ok(output);
        }

        // PUT: organization/user/{id}
        /// <summary>
        ///     Update user in the session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = id;

            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(_appContext.TenantId, user.Id))
            {
                throw new AuthorizationException();
            }
            await _userRepository.UpdateAsync(user);

            // Return.
            return NoContent();
        }

        // POST: organization/user/{id}/change-organization-role
        /// <summary>
        ///     Set user organization role in the session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPost("{id:guid}/change-organization-role")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeOrganizationUserRoleAsync(Guid id, [FromBody] ChangeOrganizationRoleDto input)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(_appContext.TenantId, id))
            {
                throw new AuthorizationException();
            }
            await _organizationUserRepository.SetOrganizationRoleByUserIdAsync(id, input.Role);

            // Return.
            return NoContent();
        }

        // FUTURE: Remove old password from DTO
        // POST: organization/user/{id}/change-password
        /// <summary>
        ///     Set user password in the session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPost("{id:guid}/change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangePasswordAsync(Guid id, [FromBody] ChangePasswordDto input)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(_appContext.TenantId, id))
            {
                throw new AuthorizationException();
            }

            // Act.
            await _signInService.SetPasswordAsync(id, input.NewPassword);

            // Return.
            return NoContent();
        }

        // DELETE: organization/user/{id}
        /// <summary>
        ///     Delete user in the session organization.
        /// </summary>
        [Authorize(Policy = "SuperuserPolicy")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(_appContext.TenantId, id))
            {
                throw new AuthorizationException();
            }
            await _userRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
