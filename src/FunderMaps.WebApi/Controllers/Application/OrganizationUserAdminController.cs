using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.ViewModels;
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
    ///         <see cref="OrganizationUserController"/>.
    ///     </para>
    /// </remarks>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("admin/organization/{id:guid}/user")]
    public class OrganizationUserAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserAdminController(IMapper mapper, IUserRepository userRepository, IOrganizationUserRepository organizationUserRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            // FUTURE: Do in 1 call.
            var userId = await _userRepository.AddAsync(user);
            // var passwordHash = _passwordHasher.HashPassword(plainPassword);
            // await _userRepository.SetPasswordHashAsync(user, passwordHash);
            await _organizationUserRepository.AddAsync(id, userId, OrganizationRole.Reader);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(Guid id, [FromQuery] PaginationModel pagination)
        {
            // Act.
            // TODO: Single call
            var userList = new List<User>();
            await foreach (var user in _organizationUserRepository.ListAllAsync(id, pagination.Navigation))
            {
                userList.Add(await _userRepository.GetByIdAsync(user));
            }

            // Map.
            var result = _mapper.Map<IList<UserDto>>(userList);

            // Return.
            return Ok(result);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid userId, [FromBody] UserDto input)
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

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, Guid userId)
        {
            // Act.
            // TODO: Move to db
            if (!await _organizationUserRepository.IsUserInOrganization(id, userId))
            {
                throw new AuthorizationException();
            }
            await _userRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
