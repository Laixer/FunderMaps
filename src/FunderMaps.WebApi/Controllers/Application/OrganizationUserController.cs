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

namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization user operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle organization operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Route("organization/user")]
    public class OrganizationUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserController(IMapper mapper, Core.AppContext appContext, IUserRepository userRepository, IOrganizationUserRepository organizationUserRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
        }

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            // FUTURE: Do in 1 call.
            var userId = await _userRepository.AddAsync(user);
            // var passwordHash = _passwordHasher.HashPassword(plainPassword);
            // await _userRepository.SetPasswordHashAsync(user, passwordHash);
            await _organizationUserRepository.AddAsync(_appContext.TenantId, userId, OrganizationRole.Reader);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] PaginationModel pagination)
        {
            // Act.
            var userList = new List<User>();
            await foreach (var user in _organizationUserRepository.ListAllAsync(_appContext.TenantId, pagination.Navigation))
            {
                userList.Add(await _userRepository.GetByIdAsync(user));
            }

            // Map.
            var result = _mapper.Map<IList<UserDto>>(userList);

            // Return.
            return Ok(result);
        }

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPut("{id:guid}")]
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

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpDelete("{id:guid}")]
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
